using System;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal class LoupeLogProvider : MvxBaseLogProvider
    {
        /// <summary>
        /// The form of the Loupe Log.Write method we're using
        /// </summary>
        internal delegate void WriteDelegate(
            int severity,
            string logSystem,
            int skipFrames,
            Exception exception,
            bool attributeToException,
            int writeMode,
            string detailsXml,
            string category,
            string caption,
            string description,
            params object[] args
            );

        private readonly WriteDelegate _logWriteDelegate;

        public LoupeLogProvider()
        {
            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("Gibraltar.Agent.Log (Loupe) not found");
            }

            _logWriteDelegate = GetLogWriteDelegate();
        }

        protected override Logger GetLogger(string name)
            => new LoupeLogger(name, _logWriteDelegate).Log;

        public static bool IsLoggerAvailable()
            => GetLogManagerType() != null;
       
        private static Type GetLogManagerType()
            => Type.GetType("Gibraltar.Agent.Log, Gibraltar.Agent");

        private static WriteDelegate GetLogWriteDelegate()
        {
            Type logManagerType = GetLogManagerType();
            Type logMessageSeverityType = Type.GetType("Gibraltar.Agent.LogMessageSeverity, Gibraltar.Agent");
            Type logWriteModeType = Type.GetType("Gibraltar.Agent.LogWriteMode, Gibraltar.Agent");

            MethodInfo method = logManagerType.GetMethodPortable(
                "Write",
                logMessageSeverityType, typeof(string), typeof(int), typeof(Exception), typeof(bool),
                logWriteModeType, typeof(string), typeof(string), typeof(string), typeof(string), typeof(object[]));

            var callDelegate = (WriteDelegate)method.CreateDelegate(typeof(WriteDelegate));
            return callDelegate;
        }

        internal class LoupeLogger
        {
            private const string LogSystem = "LibLog";

            private readonly string _category;
            private readonly WriteDelegate _logWriteDelegate;
            private readonly int _skipLevel;

            internal LoupeLogger(string category, WriteDelegate logWriteDelegate)
            {
                _category = category;
                _logWriteDelegate = logWriteDelegate;
#if DEBUG
                _skipLevel = 2;
#else
                _skipLevel = 1;
#endif
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                if (messageFunc == null)
                {
                    //nothing to log..
                    return true;
                }

                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);

                _logWriteDelegate(ToLogMessageSeverity(logLevel), LogSystem, _skipLevel, exception, true, 0, null,
                    _category, null, messageFunc.Invoke());

                return true;
            }

            private static int ToLogMessageSeverity(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Trace:
                        return TraceEventTypeValues.Verbose;
                    case MvxLogLevel.Debug:
                        return TraceEventTypeValues.Verbose;
                    case MvxLogLevel.Info:
                        return TraceEventTypeValues.Information;
                    case MvxLogLevel.Warn:
                        return TraceEventTypeValues.Warning;
                    case MvxLogLevel.Error:
                        return TraceEventTypeValues.Error;
                    case MvxLogLevel.Fatal:
                        return TraceEventTypeValues.Critical;
                    default:
                        throw new ArgumentOutOfRangeException("logLevel");
                }
            }
        }
    }
}

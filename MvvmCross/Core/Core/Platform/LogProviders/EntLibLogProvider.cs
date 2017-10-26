using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal class EntLibLogProvider : MvxBaseLogProvider
    {
        private const string TypeTemplate = "Microsoft.Practices.EnterpriseLibrary.Logging.{0}, Microsoft.Practices.EnterpriseLibrary.Logging";
        private static readonly Type LogEntryType;
        private static readonly Type LoggerType;
        private static readonly Type TraceEventTypeType;
        private static readonly Action<string, string, int> WriteLogEntry;
        private static readonly Func<string, int, bool> ShouldLogEntry;

        static EntLibLogProvider()
        {
            LogEntryType = Type.GetType(string.Format(CultureInfo.InvariantCulture, TypeTemplate, "LogEntry"));
            LoggerType = Type.GetType(string.Format(CultureInfo.InvariantCulture, TypeTemplate, "Logger"));
            TraceEventTypeType = TraceEventTypeValues.Type;
            if (LogEntryType == null || 
                TraceEventTypeType == null || 
                LoggerType == null) return;
            
            WriteLogEntry = GetWriteLogEntry();
            ShouldLogEntry = GetShouldLogEntry();
        }

        public EntLibLogProvider()
        {
            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("Microsoft.Practices.EnterpriseLibrary.Logging.Logger not found");
            }
        }

        protected override Logger GetLogger(string name)
            => new EntLibLogger(name, WriteLogEntry, ShouldLogEntry).Log;
       
        internal static bool IsLoggerAvailable()
            => TraceEventTypeType != null && LogEntryType != null;

        private static Action<string, string, int> GetWriteLogEntry()
        {
            // new LogEntry(...)
            var logNameParameter = Expression.Parameter(typeof(string), "logName");
            var messageParameter = Expression.Parameter(typeof(string), "message");
            var severityParameter = Expression.Parameter(typeof(int), "severity");

            MemberInitExpression memberInit = GetWriteLogExpression(
                messageParameter,
                Expression.Convert(severityParameter, TraceEventTypeType),
                logNameParameter);

            //Logger.Write(new LogEntry(....));
            MethodInfo writeLogEntryMethod = LoggerType.GetMethodPortable("Write", LogEntryType);
            var writeLogEntryExpression = Expression.Call(writeLogEntryMethod, memberInit);

            return Expression.Lambda<Action<string, string, int>>(
                writeLogEntryExpression,
                logNameParameter,
                messageParameter,
                severityParameter).Compile();
        }

        private static Func<string, int, bool> GetShouldLogEntry()
        {
            // new LogEntry(...)
            var logNameParameter = Expression.Parameter(typeof(string), "logName");
            var severityParameter = Expression.Parameter(typeof(int), "severity");

            MemberInitExpression memberInit = GetWriteLogExpression(
                Expression.Constant("***dummy***"),
                Expression.Convert(severityParameter, TraceEventTypeType),
                logNameParameter);

            //Logger.Write(new LogEntry(....));
            MethodInfo writeLogEntryMethod = LoggerType.GetMethodPortable("ShouldLog", LogEntryType);
            var writeLogEntryExpression = Expression.Call(writeLogEntryMethod, memberInit);

            return Expression.Lambda<Func<string, int, bool>>(
                writeLogEntryExpression,
                logNameParameter,
                severityParameter).Compile();
        }

        private static MemberInitExpression GetWriteLogExpression(Expression message,
            Expression severityParameter, ParameterExpression logNameParameter)
        {
            var entryType = LogEntryType;
            MemberInitExpression memberInit = Expression.MemberInit(Expression.New(entryType),
                Expression.Bind(entryType.GetPropertyPortable("Message"), message),
                Expression.Bind(entryType.GetPropertyPortable("Severity"), severityParameter),
                Expression.Bind(
                    entryType.GetPropertyPortable("TimeStamp"),
                    Expression.Property(null, typeof(DateTime).GetPropertyPortable("UtcNow"))),
                Expression.Bind(
                    entryType.GetPropertyPortable("Categories"),
                    Expression.ListInit(
                        Expression.New(typeof(List<string>)),
                        typeof(List<string>).GetMethodPortable("Add", typeof(string)),
                        logNameParameter)));
            return memberInit;
        }

        internal class EntLibLogger
        {
            private readonly string _loggerName;
            private readonly Action<string, string, int> _writeLog;
            private readonly Func<string, int, bool> _shouldLog;

            internal EntLibLogger(string loggerName, Action<string, string, int> writeLog, Func<string, int, bool> shouldLog)
            {
                _loggerName = loggerName;
                _writeLog = writeLog;
                _shouldLog = shouldLog;
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                var severity = MapSeverity(logLevel);
                if (messageFunc == null)
                {
                    return _shouldLog(_loggerName, severity);
                }


                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);
                if (exception != null)
                {
                    return LogException(logLevel, messageFunc, exception);
                }
                _writeLog(_loggerName, messageFunc(), severity);
                return true;
            }

            public bool LogException(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception)
            {
                var severity = MapSeverity(logLevel);
                var message = messageFunc() + Environment.NewLine + exception;
                _writeLog(_loggerName, message, severity);
                return true;
            }

            private static int MapSeverity(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Fatal:
                        return TraceEventTypeValues.Critical;
                    case MvxLogLevel.Error:
                        return TraceEventTypeValues.Error;
                    case MvxLogLevel.Warn:
                        return TraceEventTypeValues.Warning;
                    case MvxLogLevel.Info:
                        return TraceEventTypeValues.Information;
                    default:
                        return TraceEventTypeValues.Verbose;
                }
            }
        }
    }
}

using System;
using MvvmCross.Platform.Logging.LogProviders;

namespace MvvmCross.Platform.Logging
{
    internal class MvxLog : IMvxLog
    {
        internal const string FailedToGenerateLogMessage = "Failed to generate log message";

        internal static IMvxLog Instance { get; set; }

        private readonly Logger _logger;

        internal MvxLog(Logger logger)
        {
            _logger = logger;
        }

        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            if (messageFunc == null)
                return _logger(logLevel, null);

            Func<string> wrappedMessageFunc = () =>
            {
                try
                {
                    return messageFunc();
                }
                catch (Exception ex)
                {
                    Log(MvxLogLevel.Error, () => FailedToGenerateLogMessage, ex);
                }

                return null;
            };

            return _logger(logLevel, wrappedMessageFunc, exception, formatParameters);
        }
    }
}

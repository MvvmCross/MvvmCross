using System;

namespace MvvmCross.Platform.Logging
{
    public static class MvxLogExtensions
    {
        public static bool IsDebugEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Debug, null);
        }

        public static bool IsErrorEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Error, null);
        }

        public static bool IsFatalEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Fatal, null);
        }

        public static bool IsInfoEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Info, null);
        }

        public static bool IsTraceEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Trace, null);
        }

        public static bool IsWarnEnabled(this IMvxLog logger)
        {
            GuardAgainstNullLogger(logger);
            return logger.Log(MvxLogLevel.Warn, null);
        }

        public static void Debug(this IMvxLog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(MvxLogLevel.Debug, messageFunc);
        }

        public static void Debug(this IMvxLog logger, string message)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(MvxLogLevel.Debug, message.AsFunc());
            }
        }

        public static void Debug(this IMvxLog logger, string message, params object[] args)
        {
            logger.DebugFormat(message, args);
        }

        public static void Debug(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.DebugException(message, exception, args);
        }

        public static void DebugFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsDebugEnabled())
            {
                logger.LogFormat(MvxLogLevel.Debug, message, args);
            }
        }

        public static void DebugException(this IMvxLog logger, string message, Exception exception)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(MvxLogLevel.Debug, message.AsFunc(), exception);
            }
        }

        public static void DebugException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsDebugEnabled())
            {
                logger.Log(MvxLogLevel.Debug, message.AsFunc(), exception, formatParams);
            }
        }

        public static void Error(this IMvxLog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(MvxLogLevel.Error, messageFunc);
        }

        public static void Error(this IMvxLog logger, string message)
        {
            if (logger.IsErrorEnabled())
            {
                logger.Log(MvxLogLevel.Error, message.AsFunc());
            }
        }

        public static void Error(this IMvxLog logger, string message, params object[] args)
        {
            logger.ErrorFormat(message, args);
        }

        public static void Error(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.ErrorException(message, exception, args);
        }

        public static void ErrorFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsErrorEnabled())
            {
                logger.LogFormat(MvxLogLevel.Error, message, args);
            }
        }

        public static void ErrorException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsErrorEnabled())
            {
                logger.Log(MvxLogLevel.Error, message.AsFunc(), exception, formatParams);
            }
        }

        public static void Fatal(this IMvxLog logger, Func<string> messageFunc)
        {
            logger.Log(MvxLogLevel.Fatal, messageFunc);
        }

        public static void Fatal(this IMvxLog logger, string message)
        {
            if (logger.IsFatalEnabled())
            {
                logger.Log(MvxLogLevel.Fatal, message.AsFunc());
            }
        }

        public static void Fatal(this IMvxLog logger, string message, params object[] args)
        {
            logger.FatalFormat(message, args);
        }

        public static void Fatal(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.FatalException(message, exception, args);
        }

        public static void FatalFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsFatalEnabled())
            {
                logger.LogFormat(MvxLogLevel.Fatal, message, args);
            }
        }

        public static void FatalException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsFatalEnabled())
            {
                logger.Log(MvxLogLevel.Fatal, message.AsFunc(), exception, formatParams);
            }
        }

        public static void Info(this IMvxLog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(MvxLogLevel.Info, messageFunc);
        }

        public static void Info(this IMvxLog logger, string message)
        {
            if (logger.IsInfoEnabled())
            {
                logger.Log(MvxLogLevel.Info, message.AsFunc());
            }
        }

        public static void Info(this IMvxLog logger, string message, params object[] args)
        {
            logger.InfoFormat(message, args);
        }

        public static void Info(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.InfoException(message, exception, args);
        }

        public static void InfoFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsInfoEnabled())
            {
                logger.LogFormat(MvxLogLevel.Info, message, args);
            }
        }

        public static void InfoException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsInfoEnabled())
            {
                logger.Log(MvxLogLevel.Info, message.AsFunc(), exception, formatParams);
            }
        }

        public static void Trace(this IMvxLog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(MvxLogLevel.Trace, messageFunc);
        }

        public static void Trace(this IMvxLog logger, string message)
        {
            if (logger.IsTraceEnabled())
            {
                logger.Log(MvxLogLevel.Trace, message.AsFunc());
            }
        }

        public static void Trace(this IMvxLog logger, string message, params object[] args)
        {
            logger.TraceFormat(message, args);
        }

        public static void Trace(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.TraceException(message, exception, args);
        }

        public static void TraceFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsTraceEnabled())
            {
                logger.LogFormat(MvxLogLevel.Trace, message, args);
            }
        }

        public static void TraceException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsTraceEnabled())
            {
                logger.Log(MvxLogLevel.Trace, message.AsFunc(), exception, formatParams);
            }
        }

        public static void Warn(this IMvxLog logger, Func<string> messageFunc)
        {
            GuardAgainstNullLogger(logger);
            logger.Log(MvxLogLevel.Warn, messageFunc);
        }

        public static void Warn(this IMvxLog logger, string message)
        {
            if (logger.IsWarnEnabled())
            {
                logger.Log(MvxLogLevel.Warn, message.AsFunc());
            }
        }

        public static void Warn(this IMvxLog logger, string message, params object[] args)
        {
            logger.WarnFormat(message, args);
        }

        public static void Warn(this IMvxLog logger, Exception exception, string message, params object[] args)
        {
            logger.WarnException(message, exception, args);
        }

        public static void WarnFormat(this IMvxLog logger, string message, params object[] args)
        {
            if (logger.IsWarnEnabled())
            {
                logger.LogFormat(MvxLogLevel.Warn, message, args);
            }
        }

        public static void WarnException(this IMvxLog logger, string message, Exception exception, params object[] formatParams)
        {
            if (logger.IsWarnEnabled())
            {
                logger.Log(MvxLogLevel.Warn, message.AsFunc(), exception, formatParams);
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void GuardAgainstNullLogger(IMvxLog logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
        }

        private static void LogFormat(this IMvxLog logger, MvxLogLevel logLevel, string message, params object[] args)
        {
            logger.Log(logLevel, message.AsFunc(), null, args);
        }

        // Avoid the closure allocation, see https://gist.github.com/AArnott/d285feef75c18f6ecd2b
        private static Func<T> AsFunc<T>(this T value) where T : class
        {
            return value.Return;
        }

        private static T Return<T>(this T value)
        {
            return value;
        }
    }
}

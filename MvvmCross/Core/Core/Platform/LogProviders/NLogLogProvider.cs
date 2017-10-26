using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal class NLogLogProvider : MvxBaseLogProvider
    {
        private readonly Func<string, object> _getLoggerByNameDelegate;

        public NLogLogProvider()
        {
            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("NLog.LogManager not found");
            }
            _getLoggerByNameDelegate = GetGetLoggerMethodCall();
        }

        protected override Logger GetLogger(string name)
            => new NLogLogger(_getLoggerByNameDelegate(name)).Log;

        public static bool IsLoggerAvailable()
            => GetLogManagerType() != null;

        protected override OpenNdc GetOpenNdcMethod()
        {
            Type ndcContextType = Type.GetType("NLog.NestedDiagnosticsContext, NLog");
            MethodInfo pushMethod = ndcContextType.GetMethodPortable("Push", typeof(string));
            ParameterExpression messageParam = Expression.Parameter(typeof(string), "message");
            MethodCallExpression pushMethodCall = Expression.Call(null, pushMethod, messageParam);
            return Expression.Lambda<OpenNdc>(pushMethodCall, messageParam).Compile();
        }

        protected override OpenMdc GetOpenMdcMethod()
        {
            Type mdcContextType = Type.GetType("NLog.MappedDiagnosticsContext, NLog");

            MethodInfo setMethod = mdcContextType.GetMethodPortable("Set", typeof(string), typeof(string));
            MethodInfo removeMethod = mdcContextType.GetMethodPortable("Remove", typeof(string));
            ParameterExpression keyParam = Expression.Parameter(typeof(string), "key");
            ParameterExpression valueParam = Expression.Parameter(typeof(string), "value");

            MethodCallExpression setMethodCall = Expression.Call(null, setMethod, keyParam, valueParam);
            MethodCallExpression removeMethodCall = Expression.Call(null, removeMethod, keyParam);

            Action<string, string> set = Expression
                .Lambda<Action<string, string>>(setMethodCall, keyParam, valueParam)
                .Compile();
            Action<string> remove = Expression
                .Lambda<Action<string>>(removeMethodCall, keyParam)
                .Compile();

            return (key, value) =>
            {
                set(key, value);
                return new DisposableAction(() => remove(key));
            };
        }

        private static Type GetLogManagerType()
            => Type.GetType("NLog.LogManager, NLog");

        private static Func<string, object> GetGetLoggerMethodCall()
        {
            Type logManagerType = GetLogManagerType();
            MethodInfo method = logManagerType.GetMethodPortable("GetLogger", typeof(string));
            ParameterExpression nameParam = Expression.Parameter(typeof(string), "name");
            MethodCallExpression methodCall = Expression.Call(null, method, nameParam);
            return Expression.Lambda<Func<string, object>>(methodCall, nameParam).Compile();
        }

        internal class NLogLogger
        {
            private readonly dynamic _logger;

            private static Func<string, object, string, Exception, object> _logEventInfoFact;

            private static readonly object _levelTrace;
            private static readonly object _levelDebug;
            private static readonly object _levelInfo;
            private static readonly object _levelWarn;
            private static readonly object _levelError;
            private static readonly object _levelFatal;

            static NLogLogger()
            {
                try
                {
                    var logEventLevelType = Type.GetType("NLog.LogLevel, NLog");
                    if (logEventLevelType == null)
                    {
                        throw new InvalidOperationException("Type NLog.LogLevel was not found.");
                    }

                    var levelFields = logEventLevelType.GetFieldsPortable().ToList();
                    _levelTrace = levelFields.First(x => x.Name == "Trace").GetValue(null);
                    _levelDebug = levelFields.First(x => x.Name == "Debug").GetValue(null);
                    _levelInfo = levelFields.First(x => x.Name == "Info").GetValue(null);
                    _levelWarn = levelFields.First(x => x.Name == "Warn").GetValue(null);
                    _levelError = levelFields.First(x => x.Name == "Error").GetValue(null);
                    _levelFatal = levelFields.First(x => x.Name == "Fatal").GetValue(null);

                    var logEventInfoType = Type.GetType("NLog.LogEventInfo, NLog");
                    if (logEventInfoType == null)
                    {
                        throw new InvalidOperationException("Type NLog.LogEventInfo was not found.");
                    }
                    MethodInfo createLogEventInfoMethodInfo = logEventInfoType.GetMethodPortable("Create",
                        logEventLevelType, typeof(string), typeof(Exception), typeof(IFormatProvider), typeof(string), typeof(object[]));
                    ParameterExpression loggerNameParam = Expression.Parameter(typeof(string));
                    ParameterExpression levelParam = Expression.Parameter(typeof(object));
                    ParameterExpression messageParam = Expression.Parameter(typeof(string));
                    ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception));
                    UnaryExpression levelCast = Expression.Convert(levelParam, logEventLevelType);
                    MethodCallExpression createLogEventInfoMethodCall = Expression.Call(null,
                        createLogEventInfoMethodInfo,
                        levelCast, loggerNameParam, exceptionParam,
                        Expression.Constant(null, typeof(IFormatProvider)), messageParam, Expression.Constant(null, typeof(object[])));
                    _logEventInfoFact = Expression.Lambda<Func<string, object, string, Exception, object>>(createLogEventInfoMethodCall,
                        loggerNameParam, levelParam, messageParam, exceptionParam).Compile();
                }
                catch { }
            }

            internal NLogLogger(dynamic logger)
            {
                _logger = logger;
            }

            [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                if (messageFunc == null)
                {
                    return IsLogLevelEnable(logLevel);
                }
                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);

                if (_logEventInfoFact != null)
                {
                    if (IsLogLevelEnable(logLevel))
                    {
                        var nlogLevel = TranslateLevel(logLevel);
                        _logger.Log(_logEventInfoFact(_logger.Name, nlogLevel, messageFunc(), exception));
                        return true;
                    }
                    return false;
                }

                if (exception != null)
                {
                    return LogException(logLevel, messageFunc, exception);
                }
                switch (logLevel)
                {
                    case MvxLogLevel.Debug:
                        if (_logger.IsDebugEnabled)
                        {
                            _logger.Debug(messageFunc());
                            return true;
                        }
                        break;
                    case MvxLogLevel.Info:
                        if (_logger.IsInfoEnabled)
                        {
                            _logger.Info(messageFunc());
                            return true;
                        }
                        break;
                    case MvxLogLevel.Warn:
                        if (_logger.IsWarnEnabled)
                        {
                            _logger.Warn(messageFunc());
                            return true;
                        }
                        break;
                    case MvxLogLevel.Error:
                        if (_logger.IsErrorEnabled)
                        {
                            _logger.Error(messageFunc());
                            return true;
                        }
                        break;
                    case MvxLogLevel.Fatal:
                        if (_logger.IsFatalEnabled)
                        {
                            _logger.Fatal(messageFunc());
                            return true;
                        }
                        break;
                    default:
                        if (_logger.IsTraceEnabled)
                        {
                            _logger.Trace(messageFunc());
                            return true;
                        }
                        break;
                }
                return false;
            }

            private static bool IsInTypeHierarchy(Type currentType, Type checkType)
            {
                while (currentType != null && currentType != typeof(object))
                {
                    if (currentType == checkType)
                    {
                        return true;
                    }
                    currentType = currentType.GetBaseTypePortable();
                }
                return false;
            }

            [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
            private bool LogException(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Debug:
                        if (_logger.IsDebugEnabled)
                        {
                            _logger.DebugException(messageFunc(), exception);
                            return true;
                        }
                        break;
                    case MvxLogLevel.Info:
                        if (_logger.IsInfoEnabled)
                        {
                            _logger.InfoException(messageFunc(), exception);
                            return true;
                        }
                        break;
                    case MvxLogLevel.Warn:
                        if (_logger.IsWarnEnabled)
                        {
                            _logger.WarnException(messageFunc(), exception);
                            return true;
                        }
                        break;
                    case MvxLogLevel.Error:
                        if (_logger.IsErrorEnabled)
                        {
                            _logger.ErrorException(messageFunc(), exception);
                            return true;
                        }
                        break;
                    case MvxLogLevel.Fatal:
                        if (_logger.IsFatalEnabled)
                        {
                            _logger.FatalException(messageFunc(), exception);
                            return true;
                        }
                        break;
                    default:
                        if (_logger.IsTraceEnabled)
                        {
                            _logger.TraceException(messageFunc(), exception);
                            return true;
                        }
                        break;
                }
                return false;
            }

            private bool IsLogLevelEnable(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Debug:
                        return _logger.IsDebugEnabled;
                    case MvxLogLevel.Info:
                        return _logger.IsInfoEnabled;
                    case MvxLogLevel.Warn:
                        return _logger.IsWarnEnabled;
                    case MvxLogLevel.Error:
                        return _logger.IsErrorEnabled;
                    case MvxLogLevel.Fatal:
                        return _logger.IsFatalEnabled;
                    default:
                        return _logger.IsTraceEnabled;
                }
            }

            private object TranslateLevel(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Trace:
                        return _levelTrace;
                    case MvxLogLevel.Debug:
                        return _levelDebug;
                    case MvxLogLevel.Info:
                        return _levelInfo;
                    case MvxLogLevel.Warn:
                        return _levelWarn;
                    case MvxLogLevel.Error:
                        return _levelError;
                    case MvxLogLevel.Fatal:
                        return _levelFatal;
                    default:
                        throw new ArgumentOutOfRangeException("logLevel", logLevel, null);
                }
            }
        }
    }
}

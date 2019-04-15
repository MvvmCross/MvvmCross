// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MvvmCross.Logging.LogProviders
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
            ParameterExpression messageParam = Expression.Parameter(typeof(string), "message");

            Type ndlcContextType = Type.GetType("NLog.NestedDiagnosticsLogicalContext, NLog");
            if (ndlcContextType != null)
            {
                MethodInfo pushObjectMethod = ndlcContextType.GetMethod("PushObject", new[] { typeof(object) });
                if (pushObjectMethod != null)
                {
                    // NLog 4.6 introduces PushObject with correct handling of logical callcontext (NDLC)
                    MethodCallExpression pushObjectMethodCall = Expression.Call(null, pushObjectMethod, messageParam);
                    return Expression.Lambda<OpenNdc>(pushObjectMethodCall, messageParam).Compile();
                }
            }

            Type ndcContextType = Type.GetType("NLog.NestedDiagnosticsContext, NLog");
            MethodInfo pushMethod = ndcContextType.GetMethod("Push", new[] { typeof(string) });
            MethodCallExpression pushMethodCall = Expression.Call(null, pushMethod, messageParam);
            return Expression.Lambda<OpenNdc>(pushMethodCall, messageParam).Compile();
        }

        protected override OpenMdc GetOpenMdcMethod()
        {
            ParameterExpression keyParam = Expression.Parameter(typeof(string), "key");

            Type ndlcContextType = Type.GetType("NLog.NestedDiagnosticsLogicalContext, NLog");
            if (ndlcContextType != null)
            {
                MethodInfo pushObjectMethod = ndlcContextType.GetMethod("PushObject", new[] { typeof(object) });
                if (pushObjectMethod != null)
                {
                    // NLog 4.6 introduces SetScoped with correct handling of logical callcontext (MDLC)
                    Type mdlcContextType = Type.GetType("NLog.MappedDiagnosticsLogicalContext, NLog");
                    if (mdlcContextType != null)
                    {
                        MethodInfo setScopedMethod = mdlcContextType.GetMethod("SetScoped", new[] { typeof(string), typeof(object) });
                        if (setScopedMethod != null)
                        {
                            var valueObjParam = Expression.Parameter(typeof(object), "value");
                            var setScopedMethodCall = Expression.Call(null, setScopedMethod, keyParam, valueObjParam);
                            var setMethodLambda = Expression.Lambda<Func<string, object, IDisposable>>(setScopedMethodCall, keyParam, valueObjParam).Compile();
                            return (key, value) => setMethodLambda(key, value);
                        }
                    }
                }
            }

            Type mdcContextType = Type.GetType("NLog.MappedDiagnosticsContext, NLog");
            MethodInfo setMethod = mdcContextType.GetMethod("Set", new[] { typeof(string), typeof(string) });
            MethodInfo removeMethod = mdcContextType.GetMethod("Remove", new[] { typeof(string) });
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
            MethodInfo method = logManagerType.GetMethod("GetLogger", new[] { typeof(string) });
            ParameterExpression nameParam = Expression.Parameter(typeof(string), "name");
            MethodCallExpression methodCall = Expression.Call(null, method, nameParam);
            return Expression.Lambda<Func<string, object>>(methodCall, nameParam).Compile();
        }

        internal class NLogLogger
        {
            private readonly object _logger;

            private static readonly Func<string, object, string, object[], Exception, object> _logEventInfoFact;

            private static readonly object _levelTrace;
            private static readonly object _levelDebug;
            private static readonly object _levelInfo;
            private static readonly object _levelWarn;
            private static readonly object _levelError;
            private static readonly object _levelFatal;

            private static bool _structuredLoggingEnabled;

            delegate string LoggerNameDelegate(object logger);
            delegate void LogEventDelegate(object logger, Type wrapperType, object logEvent);
            delegate bool IsEnabledDelegate(object logger);
            delegate void LogDelegate(object logger, string message);
            delegate void LogExceptionDelegate(object logger, string message, Exception exception);

            private static readonly LoggerNameDelegate _loggerNameDelegate;
            private static readonly LogEventDelegate _logEventDelegate;

            private static readonly IsEnabledDelegate _isTraceEnabledDelegate;
            private static readonly IsEnabledDelegate _isDebugEnabledDelegate;
            private static readonly IsEnabledDelegate _isInfoEnabledDelegate;
            private static readonly IsEnabledDelegate _isWarnEnabledDelegate;
            private static readonly IsEnabledDelegate _isErrorEnabledDelegate;
            private static readonly IsEnabledDelegate _isFatalEnabledDelegate;

            private static readonly LogDelegate _traceDelegate;
            private static readonly LogDelegate _debugDelegate;
            private static readonly LogDelegate _infoDelegate;
            private static readonly LogDelegate _warnDelegate;
            private static readonly LogDelegate _errorDelegate;
            private static readonly LogDelegate _fatalDelegate;

            private static readonly LogExceptionDelegate _traceExceptionDelegate;
            private static readonly LogExceptionDelegate _debugExceptionDelegate;
            private static readonly LogExceptionDelegate _infoExceptionDelegate;
            private static readonly LogExceptionDelegate _warnExceptionDelegate;
            private static readonly LogExceptionDelegate _errorExceptionDelegate;
            private static readonly LogExceptionDelegate _fatalExceptionDelegate;

            static NLogLogger()
            {
                try
                {
                    var logEventLevelType = Type.GetType("NLog.LogLevel, NLog");
                    if (logEventLevelType == null)
                    {
                        throw new InvalidOperationException("Type NLog.LogLevel was not found.");
                    }

                    var levelFields = logEventLevelType.GetFields().ToList();
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

                    var loggingEventConstructor = logEventInfoType.GetConstructor(new []
                    {
                        logEventLevelType,
                        typeof(string),
                        typeof(IFormatProvider),
                        typeof(string),
                        typeof(object[]),
                        typeof(Exception),
                    });
                    ParameterExpression loggerNameParam = Expression.Parameter(typeof(string));
                    ParameterExpression levelParam = Expression.Parameter(typeof(object));
                    ParameterExpression messageParam = Expression.Parameter(typeof(string));
                    ParameterExpression messageArgsParam = Expression.Parameter(typeof(object[]));
                    ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception));
                    UnaryExpression levelCast = Expression.Convert(levelParam, logEventLevelType);

                    NewExpression newLoggingEventExpression =
                        Expression.New(loggingEventConstructor,
                            levelCast,
                            loggerNameParam,
                            Expression.Constant(null, typeof(IFormatProvider)),
                            messageParam,
                            messageArgsParam,
                            exceptionParam
                        );

                    _logEventInfoFact = Expression.Lambda<Func<string, object, string, object[], Exception, object>>(
                        newLoggingEventExpression,
                        loggerNameParam, levelParam, messageParam, messageArgsParam, exceptionParam).Compile();

                    Type loggerType = Type.GetType("NLog.Logger, NLog");

                    _loggerNameDelegate = GetLoggerNameDelegate(loggerType);

                    _logEventDelegate = GetLogEventDelegate(loggerType, logEventInfoType);

                    _isTraceEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsTraceEnabled");
                    _isDebugEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsDebugEnabled");
                    _isInfoEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsInfoEnabled");
                    _isWarnEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsWarnEnabled");
                    _isErrorEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsErrorEnabled");
                    _isFatalEnabledDelegate = GetIsEnabledDelegate(loggerType, "IsFatalEnabled");

                    _traceDelegate = GetLogDelegate(loggerType, "Trace");
                    _debugDelegate = GetLogDelegate(loggerType, "Debug");
                    _infoDelegate = GetLogDelegate(loggerType, "Info");
                    _warnDelegate = GetLogDelegate(loggerType, "Warn");
                    _errorDelegate = GetLogDelegate(loggerType, "Error");
                    _fatalDelegate = GetLogDelegate(loggerType, "Fatal");

                    _traceExceptionDelegate = GetLogExceptionDelegate(loggerType, "TraceException");
                    _debugExceptionDelegate = GetLogExceptionDelegate(loggerType, "DebugException");
                    _infoExceptionDelegate = GetLogExceptionDelegate(loggerType, "InfoException");
                    _warnExceptionDelegate = GetLogExceptionDelegate(loggerType, "WarnException");
                    _errorExceptionDelegate = GetLogExceptionDelegate(loggerType, "ErrorException");
                    _fatalExceptionDelegate = GetLogExceptionDelegate(loggerType, "FatalException");

                    _structuredLoggingEnabled = IsStructuredLoggingEnabled();
                }
                catch { }
            }

            private static IsEnabledDelegate GetIsEnabledDelegate(Type loggerType, string propertyName)
            {
                var isEnabledPropertyInfo = loggerType.GetProperty(propertyName);
                var instanceParam = Expression.Parameter(typeof(object));
                var instanceCast = Expression.Convert(instanceParam, loggerType);
                var propertyCall = Expression.Property(instanceCast, isEnabledPropertyInfo);
                return Expression.Lambda<IsEnabledDelegate>(propertyCall, instanceParam).Compile();
            }

            private static LoggerNameDelegate GetLoggerNameDelegate(Type loggerType)
            {
                var isEnabledPropertyInfo = loggerType.GetProperty("Name");
                var instanceParam = Expression.Parameter(typeof(object));
                var instanceCast = Expression.Convert(instanceParam, loggerType);
                var propertyCall = Expression.Property(instanceCast, isEnabledPropertyInfo);
                return Expression.Lambda<LoggerNameDelegate>(propertyCall, instanceParam).Compile();
            }

            private static LogDelegate GetLogDelegate(Type loggerType, string name)
            {
                var logMethodInfo = loggerType.GetMethod(name, new Type[] { typeof(string) });
                var instanceParam = Expression.Parameter(typeof(object));
                var instanceCast = Expression.Convert(instanceParam, loggerType);
                var messageParam = Expression.Parameter(typeof(string));
                var logCall = Expression.Call(instanceCast, logMethodInfo, messageParam);
                return Expression.Lambda<LogDelegate>(logCall, instanceParam, messageParam).Compile();
            }

            private static LogEventDelegate GetLogEventDelegate(Type loggerType, Type logEventType)
            {
                var logMethodInfo = loggerType.GetMethod("Log", new Type[] { typeof(Type), logEventType });
                var instanceParam = Expression.Parameter(typeof(object));
                var instanceCast = Expression.Convert(instanceParam, loggerType);
                var loggerTypeParam = Expression.Parameter(typeof(Type));
                var logEventParam = Expression.Parameter(typeof(object));
                var logEventCast = Expression.Convert(logEventParam, logEventType);
                var logCall = Expression.Call(instanceCast, logMethodInfo, loggerTypeParam, logEventCast);
                return Expression.Lambda<LogEventDelegate>(logCall, instanceParam, loggerTypeParam, logEventParam).Compile();
            }

            private static LogExceptionDelegate GetLogExceptionDelegate(Type loggerType, string name)
            {
                var logMethodInfo = loggerType.GetMethod(name, new Type[] { typeof(string), typeof(Exception) });
                var instanceParam = Expression.Parameter(typeof(object));
                var instanceCast = Expression.Convert(instanceParam, loggerType);
                var messageParam = Expression.Parameter(typeof(string));
                var exceptionParam = Expression.Parameter(typeof(Exception));
                var logCall = Expression.Call(instanceCast, logMethodInfo, messageParam, exceptionParam);
                return Expression.Lambda<LogExceptionDelegate>(logCall, instanceParam, messageParam, exceptionParam).Compile();
            }

            internal NLogLogger(object logger)
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

                if (_logEventInfoFact != null)
                {
                    if (IsLogLevelEnable(logLevel))
                    {
                        var formatMessage = messageFunc();
                        if (!_structuredLoggingEnabled)
                        {
                            IEnumerable<string> _;
                            formatMessage =
                                LogMessageFormatter.FormatStructuredMessage(formatMessage,
                                    formatParameters,
                                    out _);
                            formatParameters = null; // Has been formatted, no need for parameters
                        }

                        var nlogLevel = TranslateLevel(logLevel);
                        var nlogEvent = _logEventInfoFact(_loggerNameDelegate(_logger), nlogLevel, formatMessage, formatParameters, exception);
                        _logEventDelegate(_logger, typeof(IMvxLog), nlogEvent);
                        return true;
                    }

                    return false;
                }

                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);
                if (exception != null)
                {
                    return LogException(logLevel, messageFunc, exception);
                }
                switch (logLevel)
                {
                    case MvxLogLevel.Debug:
                        if (_isDebugEnabledDelegate(_logger))
                        {
                            _debugDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                    case MvxLogLevel.Info:
                        if (_isInfoEnabledDelegate(_logger))
                        {
                            _infoDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                    case MvxLogLevel.Warn:
                        if (_isWarnEnabledDelegate(_logger))
                        {
                            _warnDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                    case MvxLogLevel.Error:
                        if (_isErrorEnabledDelegate(_logger))
                        {
                            _errorDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                    case MvxLogLevel.Fatal:
                        if (_isFatalEnabledDelegate(_logger))
                        {
                            _fatalDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                    default:
                        if (_isTraceEnabledDelegate(_logger))
                        {
                            _traceDelegate(_logger, messageFunc());
                            return true;
                        }

                        break;
                }

                return false;
            }

            [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
            private bool LogException(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Debug:
                        if (_isDebugEnabledDelegate(_logger))
                        {
                            _debugExceptionDelegate(_logger, messageFunc(), exception);
                            return true;
                        }

                        break;
                    case MvxLogLevel.Info:
                        if (_isInfoEnabledDelegate(_logger))
                        {
                            _infoExceptionDelegate(_logger, messageFunc(), exception);
                            return true;
                        }

                        break;
                    case MvxLogLevel.Warn:
                        if (_isWarnEnabledDelegate(_logger))
                        {
                            _warnExceptionDelegate(_logger, messageFunc(), exception);
                            return true;
                        }

                        break;
                    case MvxLogLevel.Error:
                        if (_isErrorEnabledDelegate(_logger))
                        {
                            _errorExceptionDelegate(_logger, messageFunc(), exception);
                            return true;
                        }

                        break;
                    case MvxLogLevel.Fatal:
                        if (_isFatalEnabledDelegate(_logger))
                        {
                            _fatalExceptionDelegate(_logger, messageFunc(), exception);
                            return true;
                        }

                        break;
                    default:
                        if (_isTraceEnabledDelegate(_logger))
                        {
                            _traceExceptionDelegate(_logger, messageFunc(), exception);
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
                        return _isDebugEnabledDelegate(_logger);
                    case MvxLogLevel.Info:
                        return _isInfoEnabledDelegate(_logger);
                    case MvxLogLevel.Warn:
                        return _isWarnEnabledDelegate(_logger);
                    case MvxLogLevel.Error:
                        return _isErrorEnabledDelegate(_logger);
                    case MvxLogLevel.Fatal:
                        return _isFatalEnabledDelegate(_logger);
                    default:
                        return _isTraceEnabledDelegate(_logger);
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

            private static bool IsStructuredLoggingEnabled()
            {
                Type configFactoryType = Type.GetType("NLog.Config.ConfigurationItemFactory, NLog");
                if (configFactoryType != null)
                {
                    PropertyInfo parseMessagesProperty = configFactoryType.GetProperty("ParseMessageTemplates");
                    if (parseMessagesProperty != null)
                    {
                        var defaultProperty = configFactoryType.GetProperty("Default");
                        if (defaultProperty != null)
                        {
                            var configFactoryDefault = defaultProperty.GetValue(null, null);
                            if (configFactoryDefault != null)
                            {
                                var parseMessageTemplates =
                                    parseMessagesProperty.GetValue(configFactoryDefault, null) as bool?;
                                if (parseMessageTemplates != false) return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}

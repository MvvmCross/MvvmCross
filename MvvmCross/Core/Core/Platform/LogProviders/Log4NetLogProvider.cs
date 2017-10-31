using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal class Log4NetLogProvider : MvxBaseLogProvider
    {
        private readonly Func<string, object> _getLoggerByNameDelegate;

        public Log4NetLogProvider()
        {
            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("log4net.LogManager not found");
            }

            _getLoggerByNameDelegate = GetGetLoggerMethodCall();
        }

        protected override Logger GetLogger(string name)
            => new Log4NetLogger(_getLoggerByNameDelegate(name)).Log;
       
        internal static bool IsLoggerAvailable()
            => GetLogManagerType() != null;

        protected override OpenNdc GetOpenNdcMethod()
        {
            Type logicalThreadContextType = Type.GetType("log4net.LogicalThreadContext, log4net");
            PropertyInfo stacksProperty = logicalThreadContextType.GetPropertyPortable("Stacks");
            Type logicalThreadContextStacksType = stacksProperty.PropertyType;
            PropertyInfo stacksIndexerProperty = logicalThreadContextStacksType.GetPropertyPortable("Item");
            Type stackType = stacksIndexerProperty.PropertyType;
            MethodInfo pushMethod = stackType.GetMethodPortable("Push");

            ParameterExpression messageParameter =
                Expression.Parameter(typeof(string), "message");

            // message => LogicalThreadContext.Stacks.Item["NDC"].Push(message);
            MethodCallExpression callPushBody =
                Expression.Call(
                    Expression.Property(Expression.Property(null, stacksProperty),
                                        stacksIndexerProperty,
                                        Expression.Constant("NDC")),
                    pushMethod,
                    messageParameter);

            OpenNdc result =
                Expression.Lambda<OpenNdc>(callPushBody, messageParameter)
                          .Compile();

            return result;
        }

        protected override OpenMdc GetOpenMdcMethod()
        {
            Type logicalThreadContextType = Type.GetType("log4net.LogicalThreadContext, log4net");
            PropertyInfo propertiesProperty = logicalThreadContextType.GetPropertyPortable("Properties");
            Type logicalThreadContextPropertiesType = propertiesProperty.PropertyType;
            PropertyInfo propertiesIndexerProperty = logicalThreadContextPropertiesType.GetPropertyPortable("Item");

            MethodInfo removeMethod = logicalThreadContextPropertiesType.GetMethodPortable("Remove");

            ParameterExpression keyParam = Expression.Parameter(typeof(string), "key");
            ParameterExpression valueParam = Expression.Parameter(typeof(string), "value");

            MemberExpression propertiesExpression = Expression.Property(null, propertiesProperty);

            // (key, value) => LogicalThreadContext.Properties.Item[key] = value;
            BinaryExpression setProperties = Expression.Assign(Expression.Property(propertiesExpression, propertiesIndexerProperty, keyParam), valueParam);

            // key => LogicalThreadContext.Properties.Remove(key);
            MethodCallExpression removeMethodCall = Expression.Call(propertiesExpression, removeMethod, keyParam);

            Action<string, string> set = Expression
                .Lambda<Action<string, string>>(setProperties, keyParam, valueParam)
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
            => Type.GetType("log4net.LogManager, log4net");

        private static Func<string, object> GetGetLoggerMethodCall()
        {
            Type logManagerType = GetLogManagerType();
            MethodInfo method = logManagerType.GetMethodPortable("GetLogger", typeof(string));
            ParameterExpression nameParam = Expression.Parameter(typeof(string), "name");
            MethodCallExpression methodCall = Expression.Call(null, method, nameParam);
            return Expression.Lambda<Func<string, object>>(methodCall, nameParam).Compile();
        }

        internal class Log4NetLogger
        {
            private readonly dynamic _logger;
            private static Type s_callerStackBoundaryType;
            private static readonly object CallerStackBoundaryTypeSync = new object();

            private static readonly object _levelDebug;
            private static readonly object _levelInfo;
            private static readonly object _levelWarn;
            private static readonly object _levelError;
            private static readonly object _levelFatal;
            private static readonly Func<object, object, bool> _isEnabledForDelegate;
            private static readonly Action<object, object> _logDelegate;
            private static readonly Func<object, Type, object, string, Exception, object> _createLoggingEvent;
            private static readonly Action<object, string, object> _loggingEventPropertySetter;

            static Log4NetLogger()
            {
                var logEventLevelType = Type.GetType("log4net.Core.Level, log4net");
                if (logEventLevelType == null)
                {
                    throw new InvalidOperationException("Type log4net.Core.Level was not found.");
                }

                var levelFields = logEventLevelType.GetFieldsPortable().ToList();
                _levelDebug = levelFields.First(x => x.Name == "Debug").GetValue(null);
                _levelInfo = levelFields.First(x => x.Name == "Info").GetValue(null);
                _levelWarn = levelFields.First(x => x.Name == "Warn").GetValue(null);
                _levelError = levelFields.First(x => x.Name == "Error").GetValue(null);
                _levelFatal = levelFields.First(x => x.Name == "Fatal").GetValue(null);

                // Func<object, object, bool> isEnabledFor = (logger, level) => { return ((log4net.Core.ILogger)logger).IsEnabled(level); }
                var loggerType = Type.GetType("log4net.Core.ILogger, log4net");
                if (loggerType == null)
                {
                    throw new InvalidOperationException("Type log4net.Core.ILogger, was not found.");
                }
                ParameterExpression instanceParam = Expression.Parameter(typeof(object));
                UnaryExpression instanceCast = Expression.Convert(instanceParam, loggerType);
                ParameterExpression levelParam = Expression.Parameter(typeof(object));
                UnaryExpression levelCast = Expression.Convert(levelParam, logEventLevelType);
                _isEnabledForDelegate = GetIsEnabledFor(loggerType, logEventLevelType, instanceCast, levelCast, instanceParam, levelParam);

                Type loggingEventType = Type.GetType("log4net.Core.LoggingEvent, log4net");

                _createLoggingEvent = GetCreateLoggingEvent(instanceParam, instanceCast, levelParam, levelCast, loggingEventType);

                _logDelegate = GetLogDelegate(loggerType, loggingEventType, instanceCast, instanceParam);

                _loggingEventPropertySetter = GetLoggingEventPropertySetter(loggingEventType);
            }

            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ILogger")]
            internal Log4NetLogger(dynamic logger)
            {
                _logger = logger.Logger;
            }

            private static Action<object, object> GetLogDelegate(Type loggerType, Type loggingEventType, UnaryExpression instanceCast,
                                                 ParameterExpression instanceParam)
            {
                //Action<object, object, string, Exception> Log =
                //(logger, callerStackBoundaryDeclaringType, level, message, exception) => { ((ILogger)logger).Log(new LoggingEvent(callerStackBoundaryDeclaringType, logger.Repository, logger.Name, level, message, exception)); }
                MethodInfo writeExceptionMethodInfo = loggerType.GetMethodPortable("Log",
                                                                                   loggingEventType);

                ParameterExpression loggingEventParameter =
                    Expression.Parameter(typeof(object), "loggingEvent");

                UnaryExpression loggingEventCasted =
                    Expression.Convert(loggingEventParameter, loggingEventType);

                var writeMethodExp = Expression.Call(
                    instanceCast,
                    writeExceptionMethodInfo,
                    loggingEventCasted);

                var logDelegate = Expression.Lambda<Action<object, object>>(
                                                writeMethodExp,
                                                instanceParam,
                                                loggingEventParameter).Compile();

                return logDelegate;
            }

            private static Func<object, Type, object, string, Exception, object> GetCreateLoggingEvent(ParameterExpression instanceParam, UnaryExpression instanceCast, ParameterExpression levelParam, UnaryExpression levelCast, Type loggingEventType)
            {
                ParameterExpression callerStackBoundaryDeclaringTypeParam = Expression.Parameter(typeof(Type));
                ParameterExpression messageParam = Expression.Parameter(typeof(string));
                ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception));

                PropertyInfo repositoryProperty = loggingEventType.GetPropertyPortable("Repository");
                PropertyInfo levelProperty = loggingEventType.GetPropertyPortable("Level");

                ConstructorInfo loggingEventConstructor =
                    loggingEventType.GetConstructorPortable(typeof(Type), repositoryProperty.PropertyType, typeof(string), levelProperty.PropertyType, typeof(object), typeof(Exception));

                //Func<object, object, string, Exception, object> Log =
                //(logger, callerStackBoundaryDeclaringType, level, message, exception) => new LoggingEvent(callerStackBoundaryDeclaringType, ((ILogger)logger).Repository, ((ILogger)logger).Name, (Level)level, message, exception); }
                NewExpression newLoggingEventExpression =
                    Expression.New(loggingEventConstructor,
                                   callerStackBoundaryDeclaringTypeParam,
                                   Expression.Property(instanceCast, "Repository"),
                                   Expression.Property(instanceCast, "Name"),
                                   levelCast,
                                   messageParam,
                                   exceptionParam);

                var createLoggingEvent =
                    Expression.Lambda<Func<object, Type, object, string, Exception, object>>(
                                  newLoggingEventExpression,
                                  instanceParam,
                                  callerStackBoundaryDeclaringTypeParam,
                                  levelParam,
                                  messageParam,
                                  exceptionParam)
                              .Compile();

                return createLoggingEvent;
            }

            private static Func<object, object, bool> GetIsEnabledFor(Type loggerType, Type logEventLevelType,
                                                                      UnaryExpression instanceCast,
                                                                      UnaryExpression levelCast,
                                                                      ParameterExpression instanceParam,
                                                                      ParameterExpression levelParam)
            {
                MethodInfo isEnabledMethodInfo = loggerType.GetMethodPortable("IsEnabledFor", logEventLevelType);
                MethodCallExpression isEnabledMethodCall = Expression.Call(instanceCast, isEnabledMethodInfo, levelCast);

                Func<object, object, bool> result =
                    Expression.Lambda<Func<object, object, bool>>(isEnabledMethodCall, instanceParam, levelParam)
                              .Compile();

                return result;
            }

            private static Action<object, string, object> GetLoggingEventPropertySetter(Type loggingEventType)
            {
                ParameterExpression loggingEventParameter = Expression.Parameter(typeof(object), "loggingEvent");
                ParameterExpression keyParameter = Expression.Parameter(typeof(string), "key");
                ParameterExpression valueParameter = Expression.Parameter(typeof(object), "value");

                PropertyInfo propertiesProperty = loggingEventType.GetPropertyPortable("Properties");
                PropertyInfo item = propertiesProperty.PropertyType.GetPropertyPortable("Item");

                // ((LoggingEvent)loggingEvent).Properties[key] = value;
                var body =
                    Expression.Assign(
                        Expression.Property(
                            Expression.Property(Expression.Convert(loggingEventParameter, loggingEventType),
                                                propertiesProperty), item, keyParameter), valueParameter);

                Action<object, string, object> result =
                    Expression.Lambda<Action<object, string, object>>
                              (body, loggingEventParameter, keyParameter,
                               valueParameter)
                              .Compile();

                return result;
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                if (messageFunc == null)
                {
                    return IsLogLevelEnable(logLevel);
                }

                if (!IsLogLevelEnable(logLevel))
                {
                    return false;
                }

                string message = messageFunc();

                IEnumerable<string> patternMatches;

                string formattedMessage =
                    LogMessageFormatter.FormatStructuredMessage(message,
                                                                formatParameters,
                                                                out patternMatches);

                // determine correct caller - this might change due to jit optimizations with method inlining
                if (s_callerStackBoundaryType == null)
                {
                    lock (CallerStackBoundaryTypeSync)
                        s_callerStackBoundaryType = typeof(MvxLog);
                }

                var translatedLevel = TranslateLevel(logLevel);

                object loggingEvent = _createLoggingEvent(_logger, s_callerStackBoundaryType, translatedLevel, formattedMessage, exception);

                PopulateProperties(loggingEvent, patternMatches, formatParameters);

                _logDelegate(_logger, loggingEvent);

                return true;
            }

            private void PopulateProperties(object loggingEvent, IEnumerable<string> patternMatches, object[] formatParameters)
            {
                IEnumerable<KeyValuePair<string, object>> keyToValue =
                    patternMatches.Zip(formatParameters,
                                       (key, value) => new KeyValuePair<string, object>(key, value));

                foreach (KeyValuePair<string, object> keyValuePair in keyToValue)
                {
                    _loggingEventPropertySetter(loggingEvent, keyValuePair.Key, keyValuePair.Value);
                }
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

            private bool IsLogLevelEnable(MvxLogLevel logLevel)
            {
                var level = TranslateLevel(logLevel);
                return _isEnabledForDelegate(_logger, level);
            }

            private object TranslateLevel(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Trace:
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
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }
        }
    }
}

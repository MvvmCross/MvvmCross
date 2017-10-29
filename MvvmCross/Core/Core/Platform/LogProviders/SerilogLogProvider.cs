using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal class SerilogLogProvider : MvxBaseLogProvider
    {
        private readonly Func<string, object> _getLoggerByNameDelegate;
        private static Func<string, string, IDisposable> _pushProperty;

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Serilog")]
        public SerilogLogProvider()
        {
            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("Serilog.Log not found");
            }
            _getLoggerByNameDelegate = GetForContextMethodCall();
            _pushProperty = GetPushProperty();
        }

        protected override Logger GetLogger(string name)
            => new SerilogLogger(_getLoggerByNameDelegate(name)).Log;

        internal static bool IsLoggerAvailable()
            => GetLogManagerType() != null;

        protected override OpenNdc GetOpenNdcMethod()
            => message => _pushProperty("NDC", message);

        protected override OpenMdc GetOpenMdcMethod()
            => (key, value) => _pushProperty(key, value);

        private static Func<string, string, IDisposable> GetPushProperty()
        {
            Type ndcContextType = Type.GetType("Serilog.Context.LogContext, Serilog") ??
                                  Type.GetType("Serilog.Context.LogContext, Serilog.FullNetFx");

            MethodInfo pushPropertyMethod = ndcContextType.GetMethodPortable(
                "PushProperty",
                typeof(string),
                typeof(object),
                typeof(bool));

            ParameterExpression nameParam = Expression.Parameter(typeof(string), "name");
            ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");
            ParameterExpression destructureObjectParam = Expression.Parameter(typeof(bool), "destructureObjects");
            MethodCallExpression pushPropertyMethodCall = Expression
                .Call(null, pushPropertyMethod, nameParam, valueParam, destructureObjectParam);
            var pushProperty = Expression
                .Lambda<Func<string, object, bool, IDisposable>>(
                    pushPropertyMethodCall,
                    nameParam,
                    valueParam,
                    destructureObjectParam)
                .Compile();

            return (key, value) => pushProperty(key, value, false);
        }

        private static Type GetLogManagerType()
            => Type.GetType("Serilog.Log, Serilog");

        private static Func<string, object> GetForContextMethodCall()
        {
            Type logManagerType = GetLogManagerType();
            MethodInfo method = logManagerType.GetMethodPortable("ForContext", typeof(string), typeof(object), typeof(bool));
            ParameterExpression propertyNameParam = Expression.Parameter(typeof(string), "propertyName");
            ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");
            ParameterExpression destructureObjectsParam = Expression.Parameter(typeof(bool), "destructureObjects");
            MethodCallExpression methodCall = Expression.Call(null, method, new Expression[]
            {
                propertyNameParam,
                valueParam,
                destructureObjectsParam
            });
            var func = Expression.Lambda<Func<string, object, bool, object>>(
                methodCall,
                propertyNameParam,
                valueParam,
                destructureObjectsParam)
                .Compile();
            return name => func("SourceContext", name, false);
        }

        internal class SerilogLogger
        {
            private readonly object _logger;
            private static readonly object DebugLevel;
            private static readonly object ErrorLevel;
            private static readonly object FatalLevel;
            private static readonly object InformationLevel;
            private static readonly object VerboseLevel;
            private static readonly object WarningLevel;
            private static readonly Func<object, object, bool> IsEnabled;
            private static readonly Action<object, object, string, object[]> Write;
            private static readonly Action<object, object, Exception, string, object[]> WriteException;

            [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ILogger")]
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "LogEventLevel")]
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Serilog")]
            static SerilogLogger()
            {
                var logEventLevelType = Type.GetType("Serilog.Events.LogEventLevel, Serilog");
                if (logEventLevelType == null)
                {
                    throw new InvalidOperationException("Type Serilog.Events.LogEventLevel was not found.");
                }
                DebugLevel = Enum.Parse(logEventLevelType, "Debug", false);
                ErrorLevel = Enum.Parse(logEventLevelType, "Error", false);
                FatalLevel = Enum.Parse(logEventLevelType, "Fatal", false);
                InformationLevel = Enum.Parse(logEventLevelType, "Information", false);
                VerboseLevel = Enum.Parse(logEventLevelType, "Verbose", false);
                WarningLevel = Enum.Parse(logEventLevelType, "Warning", false);

                // Func<object, object, bool> isEnabled = (logger, level) => { return ((SeriLog.ILogger)logger).IsEnabled(level); }
                var loggerType = Type.GetType("Serilog.ILogger, Serilog");
                if (loggerType == null)
                {
                    throw new InvalidOperationException("Type Serilog.ILogger was not found.");
                }
                MethodInfo isEnabledMethodInfo = loggerType.GetMethodPortable("IsEnabled", logEventLevelType);
                ParameterExpression instanceParam = Expression.Parameter(typeof(object));
                UnaryExpression instanceCast = Expression.Convert(instanceParam, loggerType);
                ParameterExpression levelParam = Expression.Parameter(typeof(object));
                UnaryExpression levelCast = Expression.Convert(levelParam, logEventLevelType);
                MethodCallExpression isEnabledMethodCall = Expression.Call(instanceCast, isEnabledMethodInfo, levelCast);
                IsEnabled = Expression.Lambda<Func<object, object, bool>>(isEnabledMethodCall, instanceParam, levelParam).Compile();

                // Action<object, object, string> Write =
                // (logger, level, message, params) => { ((SeriLog.ILoggerILogger)logger).Write(level, message, params); }
                MethodInfo writeMethodInfo = loggerType.GetMethodPortable("Write", logEventLevelType, typeof(string), typeof(object[]));
                ParameterExpression messageParam = Expression.Parameter(typeof(string));
                ParameterExpression propertyValuesParam = Expression.Parameter(typeof(object[]));
                MethodCallExpression writeMethodExp = Expression.Call(
                    instanceCast,
                    writeMethodInfo,
                    levelCast,
                    messageParam,
                    propertyValuesParam);
                var expression = Expression.Lambda<Action<object, object, string, object[]>>(
                    writeMethodExp,
                    instanceParam,
                    levelParam,
                    messageParam,
                    propertyValuesParam);
                Write = expression.Compile();

                // Action<object, object, string, Exception> WriteException =
                // (logger, level, exception, message) => { ((ILogger)logger).Write(level, exception, message, new object[]); }
                MethodInfo writeExceptionMethodInfo = loggerType.GetMethodPortable("Write",
                    logEventLevelType,
                    typeof(Exception),
                    typeof(string),
                    typeof(object[]));
                ParameterExpression exceptionParam = Expression.Parameter(typeof(Exception));
                writeMethodExp = Expression.Call(
                    instanceCast,
                    writeExceptionMethodInfo,
                    levelCast,
                    exceptionParam,
                    messageParam,
                    propertyValuesParam);
                WriteException = Expression.Lambda<Action<object, object, Exception, string, object[]>>(
                    writeMethodExp,
                    instanceParam,
                    levelParam,
                    exceptionParam,
                    messageParam,
                    propertyValuesParam).Compile();
            }

            internal SerilogLogger(object logger)
            {
                _logger = logger;
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception, params object[] formatParameters)
            {
                var translatedLevel = TranslateLevel(logLevel);
                if (messageFunc == null)
                {
                    return IsEnabled(_logger, translatedLevel);
                }

                if (!IsEnabled(_logger, translatedLevel))
                {
                    return false;
                }

                if (exception != null)
                {
                    LogException(translatedLevel, messageFunc, exception, formatParameters);
                }
                else
                {
                    LogMessage(translatedLevel, messageFunc, formatParameters);
                }

                return true;
            }

            private void LogMessage(object translatedLevel, Func<string> messageFunc, object[] formatParameters)
            {
                Write(_logger, translatedLevel, messageFunc(), formatParameters);
            }

            private void LogException(object logLevel, Func<string> messageFunc, Exception exception, object[] formatParams)
            {
                WriteException(_logger, logLevel, exception, messageFunc(), formatParams);
            }

            private static object TranslateLevel(MvxLogLevel logLevel)
            {
                switch (logLevel)
                {
                    case MvxLogLevel.Fatal:
                        return FatalLevel;
                    case MvxLogLevel.Error:
                        return ErrorLevel;
                    case MvxLogLevel.Warn:
                        return WarningLevel;
                    case MvxLogLevel.Info:
                        return InformationLevel;
                    case MvxLogLevel.Trace:
                        return VerboseLevel;
                    default:
                        return DebugLevel;
                }
            }
        }
    }
}

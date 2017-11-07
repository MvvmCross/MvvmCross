using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Core.Platform.LogProviders
{
    internal sealed class ConsoleLogProvider : MvxBaseLogProvider
    {
        private static readonly Type ConsoleType;
        private static readonly Type ConsoleColorType;
        private static readonly Action<string> ConsoleWriteLine;
        private static readonly Func<int> GetConsoleForeground;
        private static readonly Action<int> SetConsoleForeground;
        private static readonly IDictionary<MvxLogLevel, int> Colors;

        static ConsoleLogProvider()
        {
            ConsoleType = Type.GetType("System.Console");
            ConsoleColorType = ConsoleColorValues.Type;

            if (!IsLoggerAvailable())
            {
                throw new InvalidOperationException("System.Console or System.ConsoleColor type not found");
            }

            MessageFormatter = DefaultMessageFormatter;
            Colors = new Dictionary<MvxLogLevel, int>
            {
                {MvxLogLevel.Fatal, ConsoleColorValues.Red},
                {MvxLogLevel.Error, ConsoleColorValues.Yellow},
                {MvxLogLevel.Warn, ConsoleColorValues.Magenta},
                {MvxLogLevel.Info, ConsoleColorValues.White},
                {MvxLogLevel.Debug, ConsoleColorValues.Gray},
                {MvxLogLevel.Trace, ConsoleColorValues.DarkGray},
            };
            ConsoleWriteLine = GetConsoleWrite();
            GetConsoleForeground = GetGetConsoleForeground();
            SetConsoleForeground = GetSetConsoleForeground();
        }

        internal static bool IsLoggerAvailable()
            => ConsoleType != null && ConsoleColorType != null;

        protected override Logger GetLogger(string name)
            => new ColouredConsoleLogger(name, ConsoleWriteLine, GetConsoleForeground, SetConsoleForeground).Log;

        internal delegate string MessageFormatterDelegate(
            string loggerName,
            MvxLogLevel level,
            object message,
            Exception e);

        internal static MessageFormatterDelegate MessageFormatter { get; set; }

        private static string DefaultMessageFormatter(string loggerName, MvxLogLevel level, object message, Exception e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture));
            stringBuilder.Append(" ");

            // Append a readable representation of the log level
            stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));
            stringBuilder.Append("(" + loggerName + ") ");

            // Append the message
            stringBuilder.Append(message);

            // Append stack trace if there is an exception
            if (e != null)
            {
                stringBuilder.Append(Environment.NewLine).Append(e.GetType());
                stringBuilder.Append(Environment.NewLine).Append(e.Message);
                stringBuilder.Append(Environment.NewLine).Append(e.StackTrace);
            }

            return stringBuilder.ToString();
        }

        private static Action<string> GetConsoleWrite()
        {
            var messageParameter = Expression.Parameter(typeof(string), "message");

            MethodInfo writeMethod = ConsoleType.GetMethodPortable("WriteLine", typeof(string));
            var writeExpression = Expression.Call(writeMethod, messageParameter);

            return Expression.Lambda<Action<string>>(
                writeExpression, messageParameter).Compile();
        }

        private static Func<int> GetGetConsoleForeground()
        {
            MethodInfo getForeground = ConsoleType.GetPropertyPortable("ForegroundColor").GetGetMethod();
            var getForegroundExpression = Expression.Convert(Expression.Call(getForeground), typeof(int));

            return Expression.Lambda<Func<int>>(getForegroundExpression).Compile();
        }

        private static Action<int> GetSetConsoleForeground()
        {
            var colorParameter = Expression.Parameter(typeof(int), "color");

            MethodInfo setForeground = ConsoleType.GetPropertyPortable("ForegroundColor").GetSetMethod();
            var setForegroundExpression = Expression.Call(setForeground,
                Expression.Convert(colorParameter, ConsoleColorType));

            return Expression.Lambda<Action<int>>(
                setForegroundExpression, colorParameter).Compile();
        }

        public class ColouredConsoleLogger
        {
            private readonly string _name;
            private readonly Action<string> _write;
            private readonly Func<int> _getForeground;
            private readonly Action<int> _setForeground;

            public ColouredConsoleLogger(string name, Action<string> write,
                Func<int> getForeground, Action<int> setForeground)
            {
                _name = name;
                _write = write;
                _getForeground = getForeground;
                _setForeground = setForeground;
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception,
                params object[] formatParameters)
            {
                if (messageFunc == null)
                {
                    return true;
                }

                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);

                Write(logLevel, messageFunc(), exception);
                return true;
            }

            protected void Write(MvxLogLevel logLevel, string message, Exception e = null)
            {
                var formattedMessage = MessageFormatter(_name, logLevel, message, e);
                int color;

                if (Colors.TryGetValue(logLevel, out color))
                {
                    var originalColor = _getForeground();
                    try
                    {
                        _setForeground(color);
                        _write(formattedMessage);
                    }
                    finally
                    {
                        _setForeground(originalColor);
                    }
                }
                else
                {
                    _write(formattedMessage);
                }
            }
        }

        private static class ConsoleColorValues
        {
            internal static readonly Type Type;
            internal static readonly int Red;
            internal static readonly int Yellow;
            internal static readonly int Magenta;
            internal static readonly int White;
            internal static readonly int Gray;
            internal static readonly int DarkGray;

            static ConsoleColorValues()
            {
                Type = Type.GetType("System.ConsoleColor");
                if (Type == null) return;
                Red = (int)Enum.Parse(Type, "Red", false);
                Yellow = (int)Enum.Parse(Type, "Yellow", false);
                Magenta = (int)Enum.Parse(Type, "Magenta", false);
                White = (int)Enum.Parse(Type, "White", false);
                Gray = (int)Enum.Parse(Type, "Gray", false);
                DarkGray = (int)Enum.Parse(Type, "DarkGray", false);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Platform.Logging.LogProviders
{
    internal sealed class ConsoleLogProvider : MvxBaseLogProvider
    {
        private static readonly IDictionary<MvxLogLevel, ConsoleColor> Colors = new Dictionary<MvxLogLevel, ConsoleColor>
        {
            { MvxLogLevel.Fatal, ConsoleColor.Red },
            { MvxLogLevel.Error, ConsoleColor.Yellow },
            { MvxLogLevel.Warn, ConsoleColor.Magenta },
            { MvxLogLevel.Info, ConsoleColor.White },
            { MvxLogLevel.Debug, ConsoleColor.Gray },
            { MvxLogLevel.Trace, ConsoleColor.DarkGray }
        };

        protected override Logger GetLogger(string name) => new ColouredConsoleLogger(name).Log;

        private static string MessageFormatter(string loggerName, MvxLogLevel level, object message, Exception e)
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

        public class ColouredConsoleLogger
        {
            private readonly string _name;

            public ColouredConsoleLogger(string name)
            {
                _name = name;
            }

            public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception,
                params object[] formatParameters)
            {
                if (messageFunc == null) return true;

                messageFunc = LogMessageFormatter.SimulateStructuredLogging(messageFunc, formatParameters);

                Write(logLevel, messageFunc(), exception);
                return true;
            }

            protected void Write(MvxLogLevel logLevel, string message, Exception e = null)
            {
                var formattedMessage = MessageFormatter(_name, logLevel, message, e);

                if (Colors.TryGetValue(logLevel, out var color))
                {
                    var originalColor = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = color;
                        Console.WriteLine(formattedMessage);
                    }
                    finally
                    {
                        Console.ForegroundColor = originalColor;
                    }
                }
                else
                {
                    Console.WriteLine(formattedMessage);
                }
            }
        }
    }
}

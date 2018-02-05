// MvxBindingLog.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Logging.LogProviders;

[assembly: InternalsVisibleTo("MvvmCross.UnitTest")]

namespace MvvmCross
{
    internal sealed class TestLogProvider : MvxBaseLogProvider
    {
        protected override Logger GetLogger(string name) => new TestLogger(name).Log;

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
            if (e != null) {
                stringBuilder.Append(Environment.NewLine).Append(e.GetType());
                stringBuilder.Append(Environment.NewLine).Append(e.Message);
                stringBuilder.Append(Environment.NewLine).Append(e.StackTrace);
            }

            return stringBuilder.ToString();
        }

        public class TestLogger
        {
            private readonly string _name;

            public TestLogger(string name)
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

                //Console.WriteLine(formattedMessage);
            }
        }
    }
}

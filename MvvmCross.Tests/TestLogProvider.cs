// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using MvvmCross.Logging;
using MvvmCross.Logging.LogProviders;

[assembly: InternalsVisibleTo("MvvmCross.UnitTest")]

namespace MvvmCross.Tests
{
    public class TestLogProvider : MvxBaseLogProvider
    {
        private TestLogger _logger;

        public TestLogProvider(TestLogger logger)
        {
            _logger = logger;
        }

        protected override Logger GetLogger(string name)
        {
            if (_logger == null)
            {
                _logger = new DefaultTestLogger(name);
            }

            return _logger.Log;
        }

        public static string MessageFormatter(string loggerName, MvxLogLevel level, object message, Exception e)
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

        private class DefaultTestLogger : TestLogger
        {
            public DefaultTestLogger(string name)
                : base(name) { }

            protected override void Write(MvxLogLevel logLevel, string message, Exception e = null)
            {
            }
        }
    }

    public abstract class TestLogger
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

        protected abstract void Write(MvxLogLevel logLevel, string message, Exception e = null);
    }
}

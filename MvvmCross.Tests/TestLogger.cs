// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using MvvmCross.Logging;
using MvvmCross.Logging.LogProviders;

[assembly: InternalsVisibleTo("MvvmCross.UnitTest")]

namespace MvvmCross.Tests
{
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

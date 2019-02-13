using System;
using MvvmCross.Logging;
using MvvmCross.Tests;
using Xunit.Abstractions;

namespace MvvmCross.UnitTest.Common
{
    public class XunitTestLogger : TestLogger
    {
        private readonly string _name;
        private readonly ITestOutputHelper _testOuputHelper;

        public XunitTestLogger(string name, ITestOutputHelper testOuputHelper) : base(name)
        {
            _name = name;
            _testOuputHelper = testOuputHelper;
        }

        protected override void Write(MvxLogLevel logLevel, string message, Exception e = null)
        {
            try
            {
                var formatted = TestLogProvider.MessageFormatter(_name, logLevel, message, e);
                _testOuputHelper.WriteLine(formatted);
            }
            catch (InvalidOperationException)
            {
                // not ready to log yet, usually happens before test actually started
                // however, MvvmCross is already setting up things in fixture and logging
            }
        }
    }
}

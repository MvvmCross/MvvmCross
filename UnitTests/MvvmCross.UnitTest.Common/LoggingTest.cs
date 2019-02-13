using MvvmCross.Tests;
using Xunit.Abstractions;

namespace MvvmCross.UnitTest.Common
{
    public abstract class LoggingTest
    {
        protected LoggingTest(string logName, MvxTestFixture fixture, ITestOutputHelper testOuputHelper)
        {
            fixture.SetupTestLogger(new XunitTestLogger(logName, testOuputHelper));
        }
    }
}

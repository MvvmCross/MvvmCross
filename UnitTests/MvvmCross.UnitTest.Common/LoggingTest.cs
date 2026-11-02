using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Tests;
using TUnit.Core;

namespace MvvmCross.UnitTest.Common
{
    public abstract class LoggingTest
    {
        protected LoggingTest(MvxTestFixture fixture, TestContext testContext)
        {
            fixture.ServiceCollection.AddLogging(builder =>
                builder.AddProvider(new TUnitTestLoggerProvider(testContext)));
        }
    }
}

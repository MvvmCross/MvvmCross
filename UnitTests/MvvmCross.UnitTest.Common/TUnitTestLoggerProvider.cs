#nullable enable

using Microsoft.Extensions.Logging;
using TUnit.Core;

namespace MvvmCross.UnitTest.Common
{
    public sealed class TUnitTestLoggerProvider(TestContext testContext) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new TUnitTestLogger(categoryName, testContext);

        public void Dispose()
        {
        }

        private sealed class TUnitTestLogger(string name, TestContext context) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                    return;

                context.Output.WriteLine($"[{logLevel}] {name}: {formatter(state, exception)}");
                if (exception != null)
                    context.Output.WriteLine(exception.ToString());
            }
        }
    }
}

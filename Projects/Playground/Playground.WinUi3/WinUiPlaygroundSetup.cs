using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.WinUi.Core;
using Playground.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.WinUi3
{
    public class WinUiPlaygroundSetup : MvxWindowsSetup<App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}

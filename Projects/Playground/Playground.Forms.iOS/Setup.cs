using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Ios.Core;
using Playground.Forms.UI;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.Forms.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, FormsApp>
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

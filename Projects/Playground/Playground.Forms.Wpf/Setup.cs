using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Wpf.Core;
using Playground.Forms.UI;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.Forms.Wpf
{
    public class Setup : MvxFormsWpfSetup<Core.App, FormsApp>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Debug()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }
    }
}

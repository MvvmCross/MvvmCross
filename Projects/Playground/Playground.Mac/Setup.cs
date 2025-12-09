using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using Playground.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace Playground.Mac
{
    [RequiresUnreferencedCode("MvxSetup requires unreferenced code")]
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    public class Setup : MvxMacSetup<App>
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
    {
        public Setup()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

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

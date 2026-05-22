using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Hosting;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.Platforms.Mac.Views;
using MvvmCross.Plugin.Json;
using Playground.Core.ViewModels;
using Serilog;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        static AppDelegate()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(a => a.NSLog())
                .WriteTo.Async(a => a.Console())
                .WriteTo.Async(a => a.Trace())
                .CreateLogger();

            MvxMacHostBuilder.CreateBuilder()
                .StartWith<RootViewModel>()
                .ConfigureServices(services =>
                {
                    services.AddLogging(l => l.AddSerilog());
                    services.AddMvxBindings();
                    services.AddMvxJson();
                    services.AddMvxViewModels(typeof(RootViewModel).Assembly);
                    services.AddMvxMacViews(typeof(AppDelegate).Assembly);
                })
                .Build()
                .Start()
                .GetAwaiter()
                .GetResult();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

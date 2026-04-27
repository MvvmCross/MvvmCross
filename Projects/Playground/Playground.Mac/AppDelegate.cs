using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.DependencyInjection;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Hosting;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.Plugin.Json;
using Playground.Core;
using Playground.Core.ViewModels;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        public AppDelegate()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            var mainWindow = NSApplication.SharedApplication.MainWindow
                ?? new NSWindow(
                    new CGRect(0, 0, MvxWindowPresentationAttribute.DefaultWidth, MvxWindowPresentationAttribute.DefaultHeight),
                    NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable,
                    NSBackingStore.Buffered,
                    false);

            MvxMacHostBuilder.CreateBuilder(mainWindow)
                .StartWith<RootViewModel>()
                .ConfigureServices(services =>
                {
                    services.AddMvxBindings();
                    services.AddMvxJson();
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

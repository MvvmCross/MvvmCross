using AppKit;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Platform;
using MvvmCross.Mac.Views.Presenters;
using MvvmCross.Platform;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        public NSWindow Window { get; set; }

        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Window = new NSWindow(new CGRect(0, 0, 400, 300),
                      NSWindowStyle.Titled | NSWindowStyle.Resizable | NSWindowStyle.Closable,
                      NSBackingStore.Buffered, false, NSScreen.MainScreen);
            Window.Title = "Playground";
            Window.ContentView = new NSView(new CGRect(0, 0, 400, 300));

            Window.WillClose += (sender, e) =>
            {
                NSApplication.SharedApplication.Terminate(this);
            };

            var presenter = new MvxMacViewPresenter(this, Window);

            var setup = new Setup(this, presenter);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

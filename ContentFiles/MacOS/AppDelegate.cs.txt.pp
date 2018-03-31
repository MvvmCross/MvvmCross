using AppKit;
using CoreGraphics;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.ViewModels;
using ObjCRuntime;
using System;

namespace $rootnamespace$
{
    public partial class AppDelegate : MvxApplicationDelegate
    {
        NSWindow _window;

        public override void DidFinishLaunching(NSNotification notification)
        {
            _window = new NSWindow(new CGRect(200, 200, 400, 700), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled,
                                   NSBackingStore.Buffered, false, NSScreen.MainScreen);

            var setup = new Setup(this, _window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            _window.MakeKeyAndOrderFront(this);
        }
    }
}

using System;
using Foundation;
using AppKit;
using ObjCRuntime;
using MvvmCross.Mac.Views.Presenters;
using MvvmCross.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Platform;

namespace $rootnamespace$
{
    public partial class AppDelegate : MvxApplicationDelegate
    {
        NSWindow _window;

        public override void FinishedLaunching (NSObject notification)
        {
            _window = new NSWindow (new CGRect(200, 200, 400, 700), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled,
                                    NSBackingStore.Buffered, false, NSScreen.MainScreen);

            var setup = new Setup(this, _window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            _window.MakeKeyAndOrderFront (this);
        }
    }
}

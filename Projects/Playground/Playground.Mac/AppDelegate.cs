using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        public AppDelegate()
        {
            MvxWindowPresentationAttribute.DefaultWidth = 250;
            MvxWindowPresentationAttribute.DefaultHeight = 250;
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

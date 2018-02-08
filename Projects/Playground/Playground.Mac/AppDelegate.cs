using Foundation;
using MvvmCross.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Mac.Core;

namespace Playground.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        public override void DidFinishLaunching(NSNotification notification)
        {
            var setup = new Setup(this);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}

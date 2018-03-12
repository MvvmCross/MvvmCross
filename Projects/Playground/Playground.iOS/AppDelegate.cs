using Foundation;
using MvvmCross;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;

namespace Playground.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        protected override MvxIosSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            return new Setup(applicationDelegate, window);
        }
    }
}

using Foundation;
using MvvmCross;
using MvvmCross.Forms.Platform.Ios.Core;
using MvvmCross.Platform.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;

namespace Playground.Forms.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(65, 105, 225);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);

            return base.FinishedLaunching(app, options);
        }

        protected override MvxIosSetup CreateSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            return new Setup(applicationDelegate, window);
        }
    }
}

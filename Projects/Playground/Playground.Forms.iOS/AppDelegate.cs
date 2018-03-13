using Foundation;
using MvvmCross.Forms.Platform.Ios.Core;
using Playground.Forms.UI;
using UIKit;

namespace Playground.Forms.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate<MvxFormsIosSetup<Core.App, FormsApp>, Core.App, FormsApp>
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(65, 105, 225);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);

            return base.FinishedLaunching(app, options);
        }
    }
}

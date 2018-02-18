using Foundation;
using MvvmCross;
using MvvmCross.Forms.Platform.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;

namespace Playground.Forms.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(65, 105, 225);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            LoadApplication(setup.FormsApplication);

            Window.MakeKeyAndVisible();

            return base.FinishedLaunching(app, options);
        }
    }
}

using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.Platform;
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

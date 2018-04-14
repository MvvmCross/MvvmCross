using Foundation;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios;
using MvvmCross.ViewModels;
using UIKit;

namespace $rootnamespace$
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

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

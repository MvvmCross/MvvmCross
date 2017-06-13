using Foundation;
using MvvmCross.Forms.iOS;
using UIKit;

namespace PageRendererExample.UI.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : MvxFormsApplicationDelegate
    {
        private UIWindow _window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            var setup = new MvvmSetup(this, _window);
            setup.Initialize();

            _window.MakeKeyAndVisible();

            LoadApplication(setup.MvxFormsApp);

            return base.FinishedLaunching(app, options);
        }
    }
}


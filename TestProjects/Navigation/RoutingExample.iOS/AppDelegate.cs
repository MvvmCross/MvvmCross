using System;
using Foundation;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using UIKit;

namespace RoutingExample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get; set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();

            Window.MakeKeyAndVisible();
            Window.BackgroundColor = UIColor.White;

            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, string srcApp, NSObject annotation)
        {
            var normalized = Uri.UnescapeDataString(url.ToString());

            var navigationService = Mvx.Resolve<IMvxNavigationService>();

            if (navigationService.CanNavigate(normalized).Result)
                navigationService.Navigate(normalized);

            return true;
        }
    }
}



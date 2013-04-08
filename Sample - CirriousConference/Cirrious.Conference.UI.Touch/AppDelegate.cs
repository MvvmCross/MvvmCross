using Cirrious.Conference.UI.Touch.Interfaces;
using Cirrious.Conference.UI.Touch.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
using Cirrious.MvvmCross.Touch.Platform;

namespace Cirrious.Conference.UI.Touch
{
    [Register("AppDelegate")]
    public partial class AppDelegate
        : MvxApplicationDelegate
        
        
    {
        public static readonly NSString NotificationWillChangeStatusBarOrientation = new NSString("UIApplicationWillChangeStatusBarOrientationNotification");
        public static readonly NSString NotificationDidChangeStatusBarOrientation = new NSString("UIApplicationDidChangeStatusBarOrientationNotification");
        public static readonly NSString NotificationOrientationDidChange = new NSString("UIDeviceOrientationDidChangeNotification");
        public static readonly NSString NotificationFavoriteUpdated = new NSString("NotificationFavoriteUpdated");

        // class-level declarations
        UIWindow _window;

        public static bool IsPhone
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

        public static bool IsPad
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
            }
        }

        public static bool HasRetina
        {
            get
            {
                if (MonoTouch.UIKit.UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                    return (MonoTouch.UIKit.UIScreen.MainScreen.Scale == 2.0);
                else
                    return false;
            }
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var presenter = new ConferencePresenter(this, _window);

            //var presenter = 
            //    IsPad 
            //        ? (IMvxTouchViewPresenter)new TwitterTabletSearchPresenter(this, _window) 
            //        : (IMvxTouchViewPresenter);
            var setup = new Setup(this, presenter);
            setup.Initialize();

            Mvx.RegisterSingleton<ITabBarPresenterHost>(presenter);

            // start the app
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            _window.MakeKeyAndVisible();

            return true;
        }
    }
}
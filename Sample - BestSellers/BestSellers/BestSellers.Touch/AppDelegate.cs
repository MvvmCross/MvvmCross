using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BestSellers.Touch
{
    [Register("AppDelegate")]
    public partial class AppDelegate
        : MvxApplicationDelegate 
        , IMvxServiceConsumer
    {
        UIWindow _window;
		
        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            // initialize app for single screen iPhone display
            var presenter = new MvxTouchViewPresenter(this, _window);
            var setup = new Setup(this, presenter);
            setup.Initialize();

            // start the app
            var start = this.GetService<IMvxStartNavigation>();
            start.Start();

            _window.MakeKeyAndVisible();
			
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}
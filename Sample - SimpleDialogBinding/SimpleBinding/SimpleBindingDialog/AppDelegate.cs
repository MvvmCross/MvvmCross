using Cirrious.MvvmCross.Dialog.Touch.Simple;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SimpleBindingDialog
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;
        TipView viewController;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            window = new UIWindow (UIScreen.MainScreen.Bounds);

            MvxSimpleTouchDialogSetup.Initialise(typeof(Converters.Converters));

            viewController = new TipView ();
            window.RootViewController = viewController;
            window.MakeKeyAndVisible ();
			
            return true;
        }
    }
}


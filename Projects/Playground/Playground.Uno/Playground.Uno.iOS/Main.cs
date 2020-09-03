using UIKit;

namespace Playground.Uno.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, typeof(App));
		}
	}

#if DEBUG
    public class HotRestartDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, Foundation.NSDictionary launchOptions)
        {
            Windows.UI.Xaml.Application.Start(_ => new App());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
#endif
}

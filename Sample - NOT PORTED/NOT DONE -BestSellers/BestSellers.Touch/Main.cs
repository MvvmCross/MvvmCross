using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoCross.Navigation;
using MonoCross.Touch;

using BestSellers;

namespace Touch.TestContainer
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.

	[MXTouchTabletOptions(TabletLayout.SinglePane)]
	[MXTouchContainerOptions(SplashBitmap = "splash.png")]
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow _window;
		
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_window = new UIWindow(UIScreen.MainScreen.Bounds);
			
			MXTouchContainer.Initialize(new BestSellers.App(), this, _window);

			//Add some Views
			MXTouchContainer.AddView<CategoryList>(new Views.CategoryListView(), ViewPerspective.Read);
			MXTouchContainer.AddView<BookList>(new Views.BookListView(), ViewPerspective.Read);
			MXTouchContainer.AddView<Book>(new Views.BookView(), ViewPerspective.Read);
			
			MXTouchContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}


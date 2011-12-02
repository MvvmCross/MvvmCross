using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoCross.Navigation;
using MonoCross.Touch;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.Touch
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[MXTouchTabletOptions(TabletLayout.MasterPane, MasterShowsinLandscape = true, MasterShowsinPotrait = true, AllowDividerResize = false)]
	[MXTouchContainerOptions(SplashBitmap = "Images/splash.jpg")]
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			MXTouchContainer.Initialize(new CustomerManagement.App(), this, window);

			// Add Views
			MXTouchContainer.AddView<List<Customer>>(typeof(CustomerListView), ViewPerspective.Default);
			MXTouchContainer.AddView<Customer>(typeof(CustomerView), ViewPerspective.Default);
			MXTouchContainer.AddView<Customer>(typeof(CustomerEditView), ViewPerspective.Update);

			MXTouchContainer.Navigate(null, MXContainer.Instance.App.NavigateOnLoad);

			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();		
			
			return true;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

#if __UNIFIED__
using ObjCRuntime;
using Foundation;
using UIKit;
#else
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using CGRect = global::System.Drawing.RectangleF;
using CGSize = global::System.Drawing.SizeF;
using CGPoint = global::System.Drawing.PointF;
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

using JASidePanels;

namespace JASidePanelsSample
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		JASidePanelController viewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new JASidePanelController();
			viewController.ShouldDelegateAutorotateToVisiblePanel = false;

			viewController.LeftPanel = new JALeftViewController();
			viewController.CenterPanel = new UINavigationController (new JACenterViewController ());
			viewController.RightPanel = new JARightViewController();

			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}


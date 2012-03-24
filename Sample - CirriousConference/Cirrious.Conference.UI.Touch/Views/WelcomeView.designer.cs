// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("WelcomeView")]
	partial class WelcomeView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel MainLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button1 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainLabel != null) {
				MainLabel.Dispose ();
				MainLabel = null;
			}

			if (Button4 != null) {
				Button4.Dispose ();
				Button4 = null;
			}

			if (Button3 != null) {
				Button3.Dispose ();
				Button3 = null;
			}

			if (Button2 != null) {
				Button2.Dispose ();
				Button2 = null;
			}

			if (Button1 != null) {
				Button1.Dispose ();
				Button1 = null;
			}
		}
	}
}

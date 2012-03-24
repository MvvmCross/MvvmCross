// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("SessionView")]
	partial class SessionView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel Label1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageView1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageView2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView TextView1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SubLabel1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SubLabel2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton favoriteButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Label1 != null) {
				Label1.Dispose ();
				Label1 = null;
			}

			if (ImageView1 != null) {
				ImageView1.Dispose ();
				ImageView1 = null;
			}

			if (ImageView2 != null) {
				ImageView2.Dispose ();
				ImageView2 = null;
			}

			if (TextView1 != null) {
				TextView1.Dispose ();
				TextView1 = null;
			}

			if (SubLabel1 != null) {
				SubLabel1.Dispose ();
				SubLabel1 = null;
			}

			if (SubLabel2 != null) {
				SubLabel2.Dispose ();
				SubLabel2 = null;
			}

			if (favoriteButton != null) {
				favoriteButton.Dispose ();
				favoriteButton = null;
			}
		}
	}
}

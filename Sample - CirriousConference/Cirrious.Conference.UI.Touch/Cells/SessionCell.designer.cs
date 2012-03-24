// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("SessionCell")]
	partial class SessionCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel MainLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageView1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageView2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SubLabel1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel SubLabel2 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainLabel != null) {
				MainLabel.Dispose ();
				MainLabel = null;
			}

			if (ImageView1 != null) {
				ImageView1.Dispose ();
				ImageView1 = null;
			}

			if (ImageView2 != null) {
				ImageView2.Dispose ();
				ImageView2 = null;
			}

			if (SubLabel1 != null) {
				SubLabel1.Dispose ();
				SubLabel1 = null;
			}

			if (SubLabel2 != null) {
				SubLabel2.Dispose ();
				SubLabel2 = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("SessionCell2")]
	partial class SessionCell2
	{
		[Outlet]
		MonoTouch.UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView Image1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView Image2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Label1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Label2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton FavoritesButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (Image1 != null) {
				Image1.Dispose ();
				Image1 = null;
			}

			if (Image2 != null) {
				Image2.Dispose ();
				Image2 = null;
			}

			if (Label1 != null) {
				Label1.Dispose ();
				Label1 = null;
			}

			if (Label2 != null) {
				Label2.Dispose ();
				Label2 = null;
			}

			if (FavoritesButton != null) {
				FavoritesButton.Dispose ();
				FavoritesButton = null;
			}
		}
	}
}

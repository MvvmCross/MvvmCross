// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TwitterSearch.UI.Touch
{
	partial class TwitterCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView ProfileImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel PersonLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel WhenLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel MainLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProfileImageView != null) {
				ProfileImageView.Dispose ();
				ProfileImageView = null;
			}

			if (PersonLabel != null) {
				PersonLabel.Dispose ();
				PersonLabel = null;
			}

			if (WhenLabel != null) {
				WhenLabel.Dispose ();
				WhenLabel = null;
			}

			if (MainLabel != null) {
				MainLabel.Dispose ();
				MainLabel = null;
			}
		}
	}
}

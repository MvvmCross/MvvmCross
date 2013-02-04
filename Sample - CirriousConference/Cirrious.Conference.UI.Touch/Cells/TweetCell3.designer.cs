// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	partial class TweetCell3
	{
		[Outlet]
		MonoTouch.UIKit.UILabel AuthorLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel WhenLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel ContentLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ProfileImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AuthorLabel != null) {
				AuthorLabel.Dispose ();
				AuthorLabel = null;
			}

			if (WhenLabel != null) {
				WhenLabel.Dispose ();
				WhenLabel = null;
			}

			if (ContentLabel != null) {
				ContentLabel.Dispose ();
				ContentLabel = null;
			}

			if (ProfileImage != null) {
				ProfileImage.Dispose ();
				ProfileImage = null;
			}
		}
	}
}

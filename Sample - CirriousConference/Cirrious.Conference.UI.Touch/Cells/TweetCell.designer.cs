// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	partial class TweetCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView ProfileImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Name { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel When { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel Detail { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProfileImage != null) {
				ProfileImage.Dispose ();
				ProfileImage = null;
			}

			if (Name != null) {
				Name.Dispose ();
				Name = null;
			}

			if (When != null) {
				When.Dispose ();
				When = null;
			}

			if (Detail != null) {
				Detail.Dispose ();
				Detail = null;
			}
		}
	}
}

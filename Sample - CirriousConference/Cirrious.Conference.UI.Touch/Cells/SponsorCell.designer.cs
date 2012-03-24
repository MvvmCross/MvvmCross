// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("SponsorCell")]
	partial class SponsorCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView TheImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TheImage != null) {
				TheImage.Dispose ();
				TheImage = null;
			}
		}
	}
}

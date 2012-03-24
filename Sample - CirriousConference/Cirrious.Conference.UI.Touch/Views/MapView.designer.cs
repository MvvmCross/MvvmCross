// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("MapView")]
	partial class MapView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel Label1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton Button3 { get; set; }
				
		void ReleaseDesignerOutlets ()
		{
			if (Label1 != null) {
				Label1.Dispose ();
				Label1 = null;
			}

			if (Button1 != null) {
				Button1.Dispose ();
				Button1 = null;
			}

			if (Button2 != null) {
				Button2.Dispose ();
				Button2 = null;
			}

			if (Button3 != null) {
				Button3.Dispose ();
				Button3 = null;
			}
		}
	}
}

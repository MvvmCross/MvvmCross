// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Tutorial.UI.Touch
{
	partial class PullToRefreshTableCellView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel FromLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel HeaderLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel MessageLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FromLabel != null) {
				FromLabel.Dispose ();
				FromLabel = null;
			}

			if (HeaderLabel != null) {
				HeaderLabel.Dispose ();
				HeaderLabel = null;
			}

			if (MessageLabel != null) {
				MessageLabel.Dispose ();
				MessageLabel = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Tutorial.UI.Touch.Views
{
	[Register ("PullToRefreshView")]
	partial class PullToRefreshView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel NumberOfEmailsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView TableViewHolder { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NumberOfEmailsLabel != null) {
				NumberOfEmailsLabel.Dispose ();
				NumberOfEmailsLabel = null;
			}

			if (TableViewHolder != null) {
				TableViewHolder.Dispose ();
				TableViewHolder = null;
			}
		}
	}
}

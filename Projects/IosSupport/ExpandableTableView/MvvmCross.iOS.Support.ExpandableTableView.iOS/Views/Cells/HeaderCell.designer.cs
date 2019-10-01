// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace MvvmCross.iOS.Support.ExpandableTableView.iOS.Views.Cells
{
	[Register ("HeaderCell")]
	partial class HeaderCell
	{
		[Outlet]
		UIKit.UILabel MainLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MainLabel != null) {
				MainLabel.Dispose ();
				MainLabel = null;
			}
		}
	}
}

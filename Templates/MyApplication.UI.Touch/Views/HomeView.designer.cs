// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MyApplication.UI.Touch
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		MonoTouch.UIKit.UITextField KeyTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton FetchButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView ResultsTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (KeyTextField != null) {
				KeyTextField.Dispose ();
				KeyTextField = null;
			}

			if (FetchButton != null) {
				FetchButton.Dispose ();
				FetchButton = null;
			}

			if (ResultsTable != null) {
				ResultsTable.Dispose ();
				ResultsTable = null;
			}
		}
	}
}

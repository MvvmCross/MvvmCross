// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Soba.XamMac.Unified
{
	[Register ("FirstViewController")]
	partial class FirstViewController
	{
		[Outlet]
		AppKit.NSButton isOnButton { get; set; }

		[Outlet]
		AppKit.NSTextField isOnLabel { get; set; }

		[Outlet]
		AppKit.NSTextField msgLabel { get; set; }

		[Outlet]
		AppKit.NSTextField msgTextField { get; set; }

		[Outlet]
		AppKit.NSTextField queryLabel { get; set; }

		[Outlet]
		AppKit.NSSearchField querySearchField { get; set; }

		[Outlet]
		AppKit.NSTextField valLabel { get; set; }

		[Outlet]
		AppKit.NSSlider valSlider { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (isOnButton != null) {
				isOnButton.Dispose ();
				isOnButton = null;
			}

			if (isOnLabel != null) {
				isOnLabel.Dispose ();
				isOnLabel = null;
			}

			if (msgLabel != null) {
				msgLabel.Dispose ();
				msgLabel = null;
			}

			if (msgTextField != null) {
				msgTextField.Dispose ();
				msgTextField = null;
			}

			if (valLabel != null) {
				valLabel.Dispose ();
				valLabel = null;
			}

			if (valSlider != null) {
				valSlider.Dispose ();
				valSlider = null;
			}

			if (querySearchField != null) {
				querySearchField.Dispose ();
				querySearchField = null;
			}

			if (queryLabel != null) {
				queryLabel.Dispose ();
				queryLabel = null;
			}
		}
	}
}

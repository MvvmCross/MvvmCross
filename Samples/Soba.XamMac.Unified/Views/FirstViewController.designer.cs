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
		AppKit.NSTextField valLabel { get; set; }

		[Outlet]
		AppKit.NSSlider valSlider { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (isOnButton != null) {
				isOnButton.Dispose ();
				isOnButton = null;
			}

			if (valLabel != null) {
				valLabel.Dispose ();
				valLabel = null;
			}

			if (valSlider != null) {
				valSlider.Dispose ();
				valSlider = null;
			}
		}
	}
}

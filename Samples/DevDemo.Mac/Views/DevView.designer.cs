// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace DevDemo.Mac
{
	[Register ("DevView")]
	partial class DevView
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}

	[Register ("DevViewController")]
	partial class DevViewController
	{
		[Outlet]
		MonoMac.AppKit.NSButton devButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devMultiTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView devScrollTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSlider devSlider { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView devTableView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (devLabel != null) {
				devLabel.Dispose ();
				devLabel = null;
			}

			if (devTextField != null) {
				devTextField.Dispose ();
				devTextField = null;
			}

			if (devMultiTextField != null) {
				devMultiTextField.Dispose ();
				devMultiTextField = null;
			}

			if (devScrollTextField != null) {
				devScrollTextField.Dispose ();
				devScrollTextField = null;
			}

			if (devTableView != null) {
				devTableView.Dispose ();
				devTableView = null;
			}

			if (devButton != null) {
				devButton.Dispose ();
				devButton = null;
			}

			if (devSlider != null) {
				devSlider.Dispose ();
				devSlider = null;
			}
		}
	}
}

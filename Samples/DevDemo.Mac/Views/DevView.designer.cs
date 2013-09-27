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
		MonoMac.AppKit.NSCollectionView devCollectionView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devMultiTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSOutlineView devOutlineView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSlider devSlider { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devSliderText { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField devTextField { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView devTextView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView devTextView2 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (devButton != null) {
				devButton.Dispose ();
				devButton = null;
			}

			if (devCollectionView != null) {
				devCollectionView.Dispose ();
				devCollectionView = null;
			}

			if (devLabel != null) {
				devLabel.Dispose ();
				devLabel = null;
			}

			if (devMultiTextField != null) {
				devMultiTextField.Dispose ();
				devMultiTextField = null;
			}

			if (devOutlineView != null) {
				devOutlineView.Dispose ();
				devOutlineView = null;
			}

			if (devSlider != null) {
				devSlider.Dispose ();
				devSlider = null;
			}

			if (devSliderText != null) {
				devSliderText.Dispose ();
				devSliderText = null;
			}

			if (devTextField != null) {
				devTextField.Dispose ();
				devTextField = null;
			}

			if (devTextView != null) {
				devTextView.Dispose ();
				devTextView = null;
			}

			if (devTextView2 != null) {
				devTextView2.Dispose ();
				devTextView2 = null;
			}
		}
	}
}

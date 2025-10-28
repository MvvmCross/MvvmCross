// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.Mac
{
	[Register ("ModalView")]
	partial class ModalView
	{
		[Outlet]
		AppKit.NSButtonCell btnClose { get; set; }

		[Outlet]
		AppKit.NSButton btnShowModal { get; set; }

		[Outlet]
		AppKit.NSTextField lblCount { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnClose != null) {
				btnClose.Dispose ();
				btnClose = null;
			}

			if (btnShowModal != null) {
				btnShowModal.Dispose ();
				btnShowModal = null;
			}

			if (lblCount != null) {
				lblCount.Dispose ();
				lblCount = null;
			}

		}
	}
}

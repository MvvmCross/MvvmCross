// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.Mac
{
	[Register ("RootView")]
	partial class RootView
	{
		[Outlet]
		AppKit.NSButton btnChild { get; set; }

		[Outlet]
		AppKit.NSButton btnModal { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnChild != null) {
				btnChild.Dispose ();
				btnChild = null;
			}

			if (btnModal != null) {
				btnModal.Dispose ();
				btnModal = null;
			}
		}
	}
}

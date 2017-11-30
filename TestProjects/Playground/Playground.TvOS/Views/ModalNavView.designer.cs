// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.TvOS
{
	[Register ("ModalNavView")]
	partial class ModalNavView
	{
		[Outlet]
		UIKit.UIButton btnClose { get; set; }

		[Outlet]
		UIKit.UIButton btnNestedModal { get; set; }

		[Outlet]
		UIKit.UIButton btnShowChild { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnShowChild != null) {
				btnShowChild.Dispose ();
				btnShowChild = null;
			}

			if (btnClose != null) {
				btnClose.Dispose ();
				btnClose = null;
			}

			if (btnNestedModal != null) {
				btnNestedModal.Dispose ();
				btnNestedModal = null;
			}
		}
	}
}

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
	[Register ("Tab1View")]
	partial class Tab1View
	{
		[Outlet]
		UIKit.UIButton btnChild { get; set; }

		[Outlet]
		UIKit.UIButton btnModal { get; set; }

		[Outlet]
		UIKit.UIButton btnModalNav { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnModalNav != null) {
				btnModalNav.Dispose ();
				btnModalNav = null;
			}

			if (btnModal != null) {
				btnModal.Dispose ();
				btnModal = null;
			}

			if (btnChild != null) {
				btnChild.Dispose ();
				btnChild = null;
			}
		}
	}
}

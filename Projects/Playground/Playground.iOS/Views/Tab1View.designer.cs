// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.Views
{
	[Register ("Tab1View")]
	partial class Tab1View
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnChild { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnModal { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnNavModal { get; set; }

		[Outlet]
		UIKit.UIButton btnSetStar { get; set; }

		[Outlet]
		UIKit.UIButton btnStar { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnTab2 { get; set; }

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

			if (btnNavModal != null) {
				btnNavModal.Dispose ();
				btnNavModal = null;
			}

			if (btnTab2 != null) {
				btnTab2.Dispose ();
				btnTab2 = null;
			}

			if (btnStar != null) {
				btnStar.Dispose ();
				btnStar = null;
			}

			if (btnSetStar != null) {
				btnSetStar.Dispose ();
				btnSetStar = null;
			}

		}
	}
}

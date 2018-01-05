// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.Views
{
	[Register ("Tab3View")]
	partial class Tab3View
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnClose { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton btnShowStack { get; set; }

		[Outlet]
		UIKit.UIButton btnShowTabsB { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnClose != null) {
				btnClose.Dispose ();
				btnClose = null;
			}

			if (btnShowStack != null) {
				btnShowStack.Dispose ();
				btnShowStack = null;
			}

			if (btnShowTabsB != null) {
				btnShowTabsB.Dispose ();
				btnShowTabsB = null;
			}
		}
	}
}

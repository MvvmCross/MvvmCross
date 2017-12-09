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
	[Register ("SplitMasterView")]
	partial class SplitMasterView
	{
		[Outlet]
		UIKit.UIButton btnDetail { get; set; }

		[Outlet]
		UIKit.UIButton btnDetailNav { get; set; }

		[Outlet]
		UIKit.UIButton btnStackNav { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnDetailNav != null) {
				btnDetailNav.Dispose ();
				btnDetailNav = null;
			}

			if (btnStackNav != null) {
				btnStackNav.Dispose ();
				btnStackNav = null;
			}

			if (btnDetail != null) {
				btnDetail.Dispose ();
				btnDetail = null;
			}
		}
	}
}

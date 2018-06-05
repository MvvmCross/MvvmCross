// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace $rootnamespace$.Views
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		AppKit.NSButton Button { get; set; }

		[Outlet]
		AppKit.NSTextField TextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TextField != null) {
				TextField.Dispose ();
				TextField = null;
			}

			if (Button != null) {
				Button.Dispose ();
				Button = null;
			}
		}
	}
}

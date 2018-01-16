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
	[Register ("ChildView")]
	partial class ChildView
	{
		[Outlet]
		AppKit.NSButtonCell btnRoot { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnRoot != null) {
				btnRoot.Dispose ();
				btnRoot = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace TwitterSearch.UI.Mac
{
	[Register ("HomeView")]
	partial class HomeView
	{
		[Outlet]
		MonoMac.AppKit.NSTextField Edit { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton Random { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton Go { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Edit != null) {
				Edit.Dispose ();
				Edit = null;
			}

			if (Random != null) {
				Random.Dispose ();
				Random = null;
			}

			if (Go != null) {
				Go.Dispose ();
				Go = null;
			}
		}
	}

	[Register ("HomeViewController")]
	partial class HomeViewController
	{
		[Outlet]
		MonoMac.AppKit.NSButton Random { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField Edit { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton Go { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Random != null) {
				Random.Dispose ();
				Random = null;
			}

			if (Edit != null) {
				Edit.Dispose ();
				Edit = null;
			}

			if (Go != null) {
				Go.Dispose ();
				Go = null;
			}
		}
	}
}

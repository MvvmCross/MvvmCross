// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace FirstDemo.Mac
{
	[Register ("FirstViewController")]
	partial class FirstViewController
	{
		[Outlet]
		MonoMac.AppKit.NSCollectionView cvContacts { get; set; }

		[Outlet]
		MonoMac.AppKit.NSOutlineView ovContacts { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField tfCombined { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField tfFirst { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField tfLast { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTableView tvContacts { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tfCombined != null) {
				tfCombined.Dispose ();
				tfCombined = null;
			}

			if (tfFirst != null) {
				tfFirst.Dispose ();
				tfFirst = null;
			}

			if (tfLast != null) {
				tfLast.Dispose ();
				tfLast = null;
			}

			if (cvContacts != null) {
				cvContacts.Dispose ();
				cvContacts = null;
			}

			if (tvContacts != null) {
				tvContacts.Dispose ();
				tvContacts = null;
			}

			if (ovContacts != null) {
				ovContacts.Dispose ();
				ovContacts = null;
			}
		}
	}

	[Register ("FirstView")]
	partial class FirstView
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}

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
	[Register ("ToolbarWindow")]
	partial class ToolbarWindow
	{
		[Outlet]
		AppKit.NSMenuItem menuItem1 { get; set; }

		[Outlet]
		AppKit.NSMenuItem menuItem2 { get; set; }

		[Outlet]
		AppKit.NSMenuItem menuItem3 { get; set; }

		[Outlet]
		AppKit.NSMenuItem menuItemSetting { get; set; }

		[Outlet]
		AppKit.NSPopUpButton popupModes { get; set; }

		[Outlet]
		AppKit.NSTextField textTitle { get; set; }

		[Action ("ToggleSetting:")]
		partial void ToggleSetting (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (menuItem1 != null) {
				menuItem1.Dispose ();
				menuItem1 = null;
			}

			if (menuItem2 != null) {
				menuItem2.Dispose ();
				menuItem2 = null;
			}

			if (menuItem3 != null) {
				menuItem3.Dispose ();
				menuItem3 = null;
			}

			if (menuItemSetting != null) {
				menuItemSetting.Dispose ();
				menuItemSetting = null;
			}

			if (textTitle != null) {
				textTitle.Dispose ();
				textTitle = null;
			}

			if (popupModes != null) {
				popupModes.Dispose ();
				popupModes = null;
			}
		}
	}
}

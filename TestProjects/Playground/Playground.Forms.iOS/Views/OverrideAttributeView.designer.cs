// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Playground.iOS.Forms.Views
{
    [Register("OverrideAttributeView")]
    partial class OverrideAttributeView
    {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton btnTabs { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (btnClose != null)
            {
                btnClose.Dispose();
                btnClose = null;
            }

            if (btnTabs != null)
            {
                btnTabs.Dispose();
                btnTabs = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Playground.TvOS
{
    [Register ("NestedModalView")]
    partial class NestedModalView
    {
        [Outlet]
        UIKit.UIButton btnClose { get; set; }


        [Outlet]
        UIKit.UIButton btnTabs { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (btnTabs != null) {
                btnTabs.Dispose ();
                btnTabs = null;
            }
        }
    }
}
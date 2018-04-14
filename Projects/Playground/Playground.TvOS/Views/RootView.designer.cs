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
    [Register ("RootView")]
    partial class RootView
    {
        [Outlet]
        UIKit.UIButton btnChild { get; set; }


        [Outlet]
        UIKit.UIButton btnModal { get; set; }


        [Outlet]
        UIKit.UIButton btnModalAttribute { get; set; }


        [Outlet]
        UIKit.UIButton btnModalNav { get; set; }


        [Outlet]
        UIKit.UIButton btnSplitNav { get; set; }


        [Outlet]
        UIKit.UIButton btnTabs { get; set; }

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

            if (btnModalAttribute != null) {
                btnModalAttribute.Dispose ();
                btnModalAttribute = null;
            }

            if (btnModalNav != null) {
                btnModalNav.Dispose ();
                btnModalNav = null;
            }

            if (btnSplitNav != null) {
                btnSplitNav.Dispose ();
                btnSplitNav = null;
            }

            if (btnTabs != null) {
                btnTabs.Dispose ();
                btnTabs = null;
            }
        }
    }
}
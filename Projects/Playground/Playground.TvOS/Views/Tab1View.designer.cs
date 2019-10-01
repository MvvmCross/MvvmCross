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
    [Register ("Tab1View")]
    partial class Tab1View
    {
        [Outlet]
        UIKit.UIButton btnChild { get; set; }


        [Outlet]
        UIKit.UIButton btnModal { get; set; }


        [Outlet]
        UIKit.UIButton btnModalNav { get; set; }

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

            if (btnModalNav != null) {
                btnModalNav.Dispose ();
                btnModalNav = null;
            }
        }
    }
}
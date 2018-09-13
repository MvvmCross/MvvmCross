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

namespace Playground.iOS.Views
{
    [Register ("Tab1View")]
    partial class Tab1View
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnChild { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnModal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnNavModal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTab2 { get; set; }

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

            if (btnNavModal != null) {
                btnNavModal.Dispose ();
                btnNavModal = null;
            }

            if (btnTab2 != null) {
                btnTab2.Dispose ();
                btnTab2 = null;
            }
        }
    }
}
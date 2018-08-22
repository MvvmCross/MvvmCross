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
    [Register ("Tab2View")]
    partial class Tab2View
    {
        [Outlet]
        UIKit.UIButton btnClose { get; set; }


        [Outlet]
        UIKit.UIButton btnStackNav { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (btnStackNav != null) {
                btnStackNav.Dispose ();
                btnStackNav = null;
            }
        }
    }
}
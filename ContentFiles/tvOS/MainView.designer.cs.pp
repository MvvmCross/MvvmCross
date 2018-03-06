// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace $rootnamespace$.Views
{
    [Register ("MainView")]
    partial class MainView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Button != null) {
                Button.Dispose ();
                Button = null;
            }

            if (TextField != null) {
                TextField.Dispose ();
                TextField = null;
            }
        }
    }
}
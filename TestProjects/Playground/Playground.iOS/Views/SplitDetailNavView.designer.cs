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

namespace Playground.iOS.Views
{
    [Register("SplitDetailNavView")]
    partial class SplitDetailNavView
    {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        void ReleaseDesignerOutlets()
        {
            if(btnClose != null)
            {
                btnClose.Dispose();
                btnClose = null;
            }
        }
    }
}
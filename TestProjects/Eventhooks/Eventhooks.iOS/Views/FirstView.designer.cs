// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace Eventhooks.iOS.Views
{
    [Register ("FirstView")]
    partial class FirstView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SecondViewButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SecondViewButton != null) {
                SecondViewButton.Dispose ();
                SecondViewButton = null;
            }
        }
    }
}
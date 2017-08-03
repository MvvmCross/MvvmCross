// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RoutingExample.iOS
{
    [Register("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIButton BtnPrePop { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton BtnRandom { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton BtnTestA { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UIButton BtnTestB { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (BtnRandom != null)
            {
                BtnRandom.Dispose();
                BtnRandom = null;
            }

            if (BtnTestA != null)
            {
                BtnTestA.Dispose();
                BtnTestA = null;
            }

            if (BtnTestB != null)
            {
                BtnTestB.Dispose();
                BtnTestB = null;
            }

            if (BtnPrePop != null)
            {
                BtnPrePop.Dispose();
                BtnPrePop = null;
            }
        }
    }
}

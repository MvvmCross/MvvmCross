using System;
using UIKit;
using SidebarNavigation;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarMasterPresentationHint : MvxSidebarBasePresentationHint
    {
        private readonly SidebarController _sidebarController;

        public MvxSidebarMasterPresentationHint(UIViewController viewController, SidebarController sidebarController)
            : base(viewController)
        { 
            _sidebarController = sidebarController;
        }

        public override bool HandleNavigation()
        {
            if (ViewController == null || _sidebarController == null)
                return false;

            _sidebarController.ChangeContentView(ViewController);

            return true;
        }
    }
}


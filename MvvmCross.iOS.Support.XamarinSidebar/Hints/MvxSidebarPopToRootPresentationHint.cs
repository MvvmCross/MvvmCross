using System;
using UIKit;
using SidebarNavigation;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarPopToRootPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarPopToRootPresentationHint(MvxPanelEnum panel, SidebarController sidebarController, UIViewController viewController)
            : base(panel)
        { 
            SidebarController = sidebarController;
            ViewController = viewController;
        }

        protected readonly SidebarController SidebarController;
        protected readonly UIViewController ViewController;

        public override bool Navigate()
        {
            if (ViewController == null || SidebarController == null)
                return false;

            SidebarController.ChangeContentView(ViewController);

            return true;
        }
    }
}


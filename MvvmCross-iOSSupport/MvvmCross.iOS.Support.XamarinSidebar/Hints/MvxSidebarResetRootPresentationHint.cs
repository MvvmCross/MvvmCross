﻿using UIKit;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Support.XamarinSidebar.Extensions;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarResetRootPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarResetRootPresentationHint(MvxPanelEnum panel, MvxSidebarPanelController sidebarPanelController, UIViewController viewController)
            : base(panel)
        {
            SidebarPanelController = sidebarPanelController;
            ViewController = viewController;
        }

        protected readonly MvxSidebarPanelController SidebarPanelController;
        protected readonly UIViewController ViewController;

        public override bool Navigate()
        {
            if (ViewController == null || SidebarPanelController == null)
                return false;

            var navigationController = SidebarPanelController.NavigationController;

            if (navigationController == null)
                return false;

            navigationController.ViewControllers = new[] { ViewController };

            if (Panel == MvxPanelEnum.Center)
                ViewController.ShowMenuButton(SidebarPanelController);

            return true;
        }
    }
}


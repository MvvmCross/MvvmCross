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

            //if(Panel == MvxPanelEnum.Left || Panel == MvxPanelEnum.Right)
                ShowSidebarToggle();

            return true;
        }

        protected virtual void ShowSidebarToggle()
        {
            var barButtonItem = new UIBarButtonItem(UIImage.FromBundle("threelines")
                , UIBarButtonItemStyle.Plain
                , (sender, args) => SidebarController.ToggleMenu());
             
            var navigationItem = ViewController.NavigationItem;

            if (Panel == MvxPanelEnum.Left)
            {
                SidebarController.MenuLocation = MenuLocations.Left;
                ViewController.NavigationItem.SetLeftBarButtonItem(barButtonItem, true);
            }
            else
            {
                SidebarController.MenuLocation = MenuLocations.Right;    
                ViewController.NavigationItem.SetRightBarButtonItem(barButtonItem, true);
            }
        }
    }
}


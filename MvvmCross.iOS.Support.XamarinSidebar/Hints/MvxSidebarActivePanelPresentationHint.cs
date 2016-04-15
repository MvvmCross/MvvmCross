using System;
using MvvmCross.Core.ViewModels;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;
using SidebarNavigation;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarActivePanelPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarActivePanelPresentationHint(MvxPanelEnum panel, MvxSidebarPanelController sidebarPanelController, UIViewController viewController)
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

            switch(Panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    InitSidebar();
                    break;
                case MvxPanelEnum.Center:
                default:
                    navigationController?.PushViewController(ViewController, true);
                    break;
            }

            return true;
        }

        protected virtual void InitSidebar()
        {
            var sidebarController = SidebarPanelController.SidebarController;
            var barButtonItem = new UIBarButtonItem(UIImage.FromBundle("threelines")
                , UIBarButtonItemStyle.Plain
                , (sender, args) => sidebarController.ToggleMenu());
            
            var topViewController = SidebarPanelController.NavigationController.TopViewController;

            sidebarController.ChangeMenuView(ViewController);

            if (Panel == MvxPanelEnum.Left)
            {
                sidebarController.MenuLocation = MenuLocations.Left;
                topViewController.NavigationItem.SetLeftBarButtonItem(barButtonItem, true);
            }
            else
            {
                sidebarController.MenuLocation = MenuLocations.Right;    
                topViewController.NavigationItem.SetRightBarButtonItem(barButtonItem, true);
            }

            var xamarinSidebarMenu = ViewController as IXamarinSidebarMenu;
            if (xamarinSidebarMenu != null)
            {
                sidebarController.HasShadowing = xamarinSidebarMenu.HasShadowing;
                sidebarController.MenuWidth = xamarinSidebarMenu.MenuWidth;  
            }
        }
    }
}


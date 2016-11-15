using System;
using UIKit;
using SidebarNavigation;

namespace MvvmCross.iOS.Support.XamarinSidebar.Extensions
{
    public static class ViewControllerExtensions
    {
        public static void ShowMenuButton(this UIViewController viewController, MvxSidebarPanelController sidebarPanelController)
        {
            UIBarButtonItem barButtonItem;

            // Make there are currently no left or right buttons
            viewController.NavigationItem.SetLeftBarButtonItem(null, true);
            viewController.NavigationItem.SetRightBarButtonItem(null, true);

            if (sidebarPanelController.HasLeftMenu)
            {
                var mvxSidebarMenu = sidebarPanelController.LeftSidebarController.MenuAreaController as IMvxSidebarMenu;
                sidebarPanelController.LeftSidebarController.MenuLocation = MenuLocations.Left;
                barButtonItem = CreateBarButtonItem(sidebarPanelController.LeftSidebarController, mvxSidebarMenu);

                viewController.NavigationItem.SetLeftBarButtonItem(barButtonItem, true);
            }

            if (sidebarPanelController.HasRightMenu)
            {
                var mvxSidebarMenu = sidebarPanelController.RightSidebarController.MenuAreaController as IMvxSidebarMenu;
                sidebarPanelController.RightSidebarController.MenuLocation = MenuLocations.Right;
                barButtonItem = CreateBarButtonItem(sidebarPanelController.RightSidebarController, mvxSidebarMenu);


                viewController.NavigationItem.SetRightBarButtonItem(barButtonItem, true);
            }
        }

        private static UIBarButtonItem CreateBarButtonItem(SidebarController sidebarController, IMvxSidebarMenu mvxSidebarMenu = null)
        {
            UIBarButtonItem barButtonItem;

            if (mvxSidebarMenu != null)
            {
                barButtonItem = new UIBarButtonItem(mvxSidebarMenu.MenuButtonImage
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) =>
                    {
                        sidebarController.MenuWidth = mvxSidebarMenu.MenuWidth;
                        sidebarController.ViewWillAppear(false);
                        sidebarController.ToggleMenu();
                    });
            }
            else
            {
                barButtonItem = new UIBarButtonItem("Menu"
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) =>
                    {
                        sidebarController.ToggleMenu();
                    });
            }

            return barButtonItem;
        }
    }
}

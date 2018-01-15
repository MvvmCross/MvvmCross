using MvvmCross.iOS.Support.XamarinSidebar.Views;
using SidebarNavigation;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Extensions
{
    public static class ViewControllerExtensions
    {
        public static void ShowMenuButton(this UIViewController viewController, MvxSidebarViewController sidebarPanelController, bool showLeft = true, bool showRight = true)
        {
            UIBarButtonItem barButtonItem;

            if (sidebarPanelController.HasLeftMenu && showLeft)
            {
                var mvxSidebarMenu = sidebarPanelController.LeftSidebarController.MenuAreaController as IMvxSidebarMenu;
                sidebarPanelController.LeftSidebarController.MenuLocation = MenuLocations.Left;
                barButtonItem = CreateBarButtonItem(sidebarPanelController.LeftSidebarController, mvxSidebarMenu);

                viewController.NavigationItem.SetLeftBarButtonItem(barButtonItem, true);
            }

            if (sidebarPanelController.HasRightMenu && showRight)
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
                        if (sidebarController.IsOpen)
                        {
                            mvxSidebarMenu.MenuWillClose();
                        }
                        else
                        {
                            mvxSidebarMenu.MenuWillOpen();
                        }
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

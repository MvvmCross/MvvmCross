using System;
using MvvmCross.Core.ViewModels;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;
using SidebarNavigation;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarActivePanelPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarActivePanelPresentationHint(MvxPanelEnum panel, UINavigationController navigationController, SidebarController sidebarController, UIViewController viewController)
            : base(panel)
        {
            NavigationController = navigationController;
            SidebarController = sidebarController;
            ViewController = viewController;
        }

        protected readonly UINavigationController NavigationController;
        protected readonly SidebarController SidebarController;
        protected readonly UIViewController ViewController;

        public override bool Navigate()
        {
            if (ViewController == null || NavigationController == null)
                return false;

            switch(Panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    InitSidebar();
                    break;
                case MvxPanelEnum.Center:
                default:
                    NavigationController.PushViewController(ViewController, true);
                    break;
            }

            return true;
        }

        protected virtual void InitSidebar()
        {
            var barButtonItem = new UIBarButtonItem(UIImage.FromBundle("threelines")
                , UIBarButtonItemStyle.Plain
                , (sender, args) => SidebarController.ToggleMenu());

            var navigationItem = SidebarController.ChildViewControllers[0].NavigationItem;

            SidebarController.ChangeMenuView(ViewController);

            if (Panel == MvxPanelEnum.Left)
            {
                SidebarController.MenuLocation = MenuLocations.Left;
                navigationItem.SetLeftBarButtonItem(barButtonItem, true);
            }
            else
            {
                SidebarController.MenuLocation = MenuLocations.Right;    
                navigationItem.SetRightBarButtonItem(barButtonItem, true);
            }
        }
    }
}


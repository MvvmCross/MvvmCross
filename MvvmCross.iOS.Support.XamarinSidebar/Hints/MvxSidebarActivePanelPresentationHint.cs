namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    using UIKit;
    using SidePanels;
    using SidebarNavigation;

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
            var sidebarController = Panel == MvxPanelEnum.Left 
                                                         ? SidebarPanelController.LeftSidebarController 
                                                         : SidebarPanelController.RightSidebarController;

            UIBarButtonItem barButtonItem; 

            var xamarinSidebarMenu = ViewController as IMvxSidebarMenu;
            if (xamarinSidebarMenu != null)
            {
                sidebarController.HasShadowing = xamarinSidebarMenu.HasShadowing;
                sidebarController.MenuWidth = xamarinSidebarMenu.MenuWidth;

                barButtonItem = new UIBarButtonItem(xamarinSidebarMenu.MenuButtonImage
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) => sidebarController.ToggleMenu());
            }
            else
            {
                barButtonItem = new UIBarButtonItem("Menu"
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) => sidebarController.ToggleMenu());
            }


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
        }
    }
}


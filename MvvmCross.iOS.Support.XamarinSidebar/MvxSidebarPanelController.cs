using SidebarNavigation;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPanelController : UIViewController, IMvxSideMenu
    {
        private readonly UIViewController _subRootViewController;

        public MvxSidebarPanelController(UINavigationController navigationController)
        {
            _subRootViewController = new UIViewController();
            NavigationController = navigationController;
        }

        public UINavigationController NavigationController { get; private set; }
        public SidebarController LeftSidebarController { get; private set; }
        public SidebarController RightSidebarController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var initialEmptySideMenu = new UIViewController();

            LeftSidebarController = new SidebarController(_subRootViewController, NavigationController, initialEmptySideMenu);
            RightSidebarController = new SidebarController(this, _subRootViewController, initialEmptySideMenu);
        }

        public void Close()
        {
            if (LeftSidebarController != null && LeftSidebarController.IsOpen)
                LeftSidebarController.CloseMenu();

            if (RightSidebarController != null && RightSidebarController.IsOpen)
                RightSidebarController.CloseMenu();
        }

        public void Open(MvxPanelEnum panelEnum)
        {
            if (panelEnum == MvxPanelEnum.Left)
                OpenMenu(LeftSidebarController);
            else if (panelEnum == MvxPanelEnum.Right)
                OpenMenu(RightSidebarController);
        }

        private void OpenMenu(SidebarController sidebarController)
        {
            if (sidebarController != null && !sidebarController.IsOpen)
                sidebarController.OpenMenu();
        }
    }
}


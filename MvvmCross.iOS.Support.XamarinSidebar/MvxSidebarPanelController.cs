using SidebarNavigation;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPanelController : UIViewController
    {
        public MvxSidebarPanelController(UINavigationController navigationController)
        {
            NavigationController = navigationController;
        }

        public UINavigationController NavigationController { get; private set; }
        public SidebarController SidebarController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var initialEmptySideMenu = new UIViewController();

            SidebarController = new SidebarController(this, NavigationController, initialEmptySideMenu);
        }

    }
}


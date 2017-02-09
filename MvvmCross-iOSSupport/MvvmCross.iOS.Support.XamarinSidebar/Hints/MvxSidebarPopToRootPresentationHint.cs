using UIKit;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarPopToRootPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarPopToRootPresentationHint(MvxPanelEnum panel, MvxSidebarPanelController sidebarPanelController, UIViewController viewController)
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

            navigationController.PopToRootViewController(false);
            navigationController.PushViewController(ViewController, false);

            return true;
        }
    }
}


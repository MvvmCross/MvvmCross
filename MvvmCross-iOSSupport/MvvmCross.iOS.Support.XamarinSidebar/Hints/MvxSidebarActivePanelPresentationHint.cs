namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    using UIKit;
    using SidePanels;
    using SidebarNavigation;

    public class MvxSidebarActivePanelPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarActivePanelPresentationHint(MvxPanelEnum panel, IMvxSidebarViewController sidebarPanelController, UIViewController viewController)
            : base(panel)
        {
            SidebarPanelController = sidebarPanelController;
            ViewController = viewController;
        }

        protected readonly IMvxSidebarViewController SidebarPanelController;
        protected readonly UIViewController ViewController;

        public override bool Navigate()
        {
            if (ViewController == null || SidebarPanelController == null)
                return false;

            var navigationController = (SidebarPanelController as MvxSidebarViewController).NavigationController;

            switch (Panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    navigationController?.PushViewController(ViewController, true);
                    break;
            }

            return true;
        }
    }
}


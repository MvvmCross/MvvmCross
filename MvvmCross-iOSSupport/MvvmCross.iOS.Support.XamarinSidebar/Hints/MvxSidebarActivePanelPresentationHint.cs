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

            switch (Panel)
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

            var xamarinSidebarMenu = ViewController as IMvxSidebarMenu;
            if (xamarinSidebarMenu != null)
            {

                sidebarController.HasShadowing = xamarinSidebarMenu.HasShadowing;
                sidebarController.DisablePanGesture = false;
                sidebarController.StateChangeHandler += (object sender, bool e) =>
                   {
                       sidebarController.MenuWidth = xamarinSidebarMenu.MenuWidth;
                       sidebarController.ViewWillAppear(false);
                   };
            }

            sidebarController.ChangeMenuView(ViewController);
        }
    }
}


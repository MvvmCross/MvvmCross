using CoreImage;
using MvvmCross.Platform.Platform;
namespace MvvmCross.iOS.Support.XamarinSidebar
{
    using SidebarNavigation;
    using UIKit;
    using SidePanels;
    using MvvmCross.Platform;

    public class MvxSidebarPanelController : UIViewController, IMvxSideMenu
    {
        private readonly UIViewController _subRootViewController;
        private bool _isInitialized;

        public bool StatusBarHidden { get; set; }
        public bool ToggleStatusBarHiddenOnOpen { get; set; } = false;

        public MvxSidebarPanelController(UINavigationController navigationController)
        {
            _subRootViewController = new UIViewController();
            NavigationController = navigationController;
        }

        public void Initialize()
        {
            var initialEmptySideMenu = new UIViewController();

            LeftSidebarController = new SidebarController(_subRootViewController, NavigationController, initialEmptySideMenu);
            RightSidebarController = new SidebarController(this, _subRootViewController, initialEmptySideMenu);

            LeftSidebarController.StateChangeHandler += (object sender, bool e) =>
            {
                if (ToggleStatusBarHiddenOnOpen)
                    ToggleStatusBarStatus();
            };

            RightSidebarController.StateChangeHandler += (object sender, bool e) =>
            {
                if (ToggleStatusBarHiddenOnOpen)
                    ToggleStatusBarStatus();
            };

            _isInitialized = true;
        }

        public new UINavigationController NavigationController { get; private set; }
        public SidebarController LeftSidebarController { get; private set; }
        public SidebarController RightSidebarController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (!_isInitialized)
            {
                Mvx.Trace(MvxTraceLevel.Warning, "The instance of 'MvxSidebarPanelController' class is not initialized. Showing or hiding the sidemenu could show unexpected behaviour. Please call the 'Initialize()' method after constructing the 'MvxSidebarPanelController' class.");
            }
        }

        public override UIStatusBarAnimation PreferredStatusBarUpdateAnimation
        {
            get
            {
                return UIStatusBarAnimation.Fade;
            }
        }

        public override bool PrefersStatusBarHidden()
        {
            return StatusBarHidden;
        }

        public void ToggleStatusBarStatus()
        {
            UIView.Animate(0.25,
                animation: () =>
                {
                    StatusBarHidden = !StatusBarHidden;
                    SetNeedsStatusBarAppearanceUpdate();
                }
            );
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


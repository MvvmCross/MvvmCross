namespace MvvmCross.iOS.Support.XamarinSidebar
{
    using SidebarNavigation;
    using UIKit;
    using SidePanels;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using System.Linq;
    using System.Reflection;
    using System;
    using MvvmCross.iOS.Views;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.iOS.Views.Presenters;

    public class MvxSidebarPanelController : UIViewController, IMvxSideMenu
    {
        private readonly UIViewController _subRootViewController;
        private bool _isInitializing;
        private bool _isInitialized;
        private UIViewController _leftSideMenu;
        private UIViewController _rightSideMenu;

        public MvxSidebarPanelController(UINavigationController navigationController)
        {
            _subRootViewController = new UIViewController();
            NavigationController = navigationController;
        }

        public bool StatusBarHidden { get; set; }

        public bool ToggleStatusBarHiddenOnOpen { get; set; } = false;

        public new UINavigationController NavigationController { get; private set; }

        public SidebarController LeftSidebarController { get; private set; }

        public SidebarController RightSidebarController {get; private set; }

        public UIViewController LeftSideMenu
        {
            get { return _leftSideMenu; }
            set
            {
                _leftSideMenu = value;
                SetupSideMenu();
            }
        }

        public UIViewController RightSideMenu
        {
            get { return _rightSideMenu; }
            set
            {
                _rightSideMenu = value;
                SetupSideMenu();
            }
        }

        public bool HasLeftMenu => LeftSideMenu != null;

        public bool HasRightMenu => RightSideMenu != null;

        public void Initialize()
        {
            _isInitializing = true;

            try
            {
                _leftSideMenu = ResolveSideMenu(MvxPanelEnum.Left);
                _rightSideMenu = ResolveSideMenu(MvxPanelEnum.Right);

                SetupSideMenu();
            }
            finally
            {
                _isInitializing = false;
                _isInitialized = true;
            }
        }

        private void SetupSideMenu()
        {
            if (_leftSideMenu != null && _rightSideMenu != null)
            {
                LeftSidebarController = new SidebarController(_subRootViewController, NavigationController, _leftSideMenu);
                ConfigureSideMenu(_leftSideMenu, LeftSidebarController);

                RightSidebarController = new SidebarController(this, _subRootViewController, _rightSideMenu);
                ConfigureSideMenu(_rightSideMenu, RightSidebarController);
            }
            else if (_leftSideMenu != null)
            {
                LeftSidebarController = new SidebarController(this, NavigationController, _leftSideMenu);
                RightSidebarController = null;
                ConfigureSideMenu(_leftSideMenu, LeftSidebarController);
            }
            else if (_rightSideMenu != null)
            {
                LeftSidebarController = null;
                RightSidebarController = new SidebarController(this, NavigationController, _rightSideMenu);
                ConfigureSideMenu(_rightSideMenu, RightSidebarController);
            }
            else
            {
                Mvx.Trace(MvxTraceLevel.Warning, $"No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'.");
            }
        }

        private UIViewController ResolveSideMenu(MvxPanelEnum location)
        {
            var assembly = Assembly.GetEntryAssembly();

            var types = (from type in assembly.GetTypes()
                         from attribute in type.GetCustomAttributes<MvxPanelPresentationAttribute>(true)
                         where attribute.Panel == location
                        select type).ToArray();

            if (types == null || types.Length == 0)
            {
                return null;
            }

            if (types != null && types.Length > 1)
            {
                Mvx.Trace(MvxTraceLevel.Warning, $"Found more then one {location.ToString()} panel, using the first one in the array ({types[0].ToString()}).");
            }

            return CreateInstance(types[0]) as UIViewController;
        }

        private IMvxIosView CreateInstance(Type viewControllerType)
        {
            var viewModelType = GetBaseType(viewControllerType);
            var presenter = Mvx.Resolve<IMvxIosViewPresenter>();
            return presenter.CreateViewControllerFor(new MvxViewModelRequest(viewModelType, null, null, null));
        }

        private Type GetBaseType(Type type)
        {
            while (type.BaseType != null)
            {
                type = type.BaseType;
                if (type.IsGenericType)
                {
                    var viewModelType = type.GetGenericArguments().FirstOrDefault(argument => typeof(IMvxViewModel).IsAssignableFrom(argument));

                    if (viewModelType != null)
                    {
                        return viewModelType;
                    }
                }
            }

            throw new InvalidOperationException("Unable to locate ViewModel type on ViewController.");
        }

        private void ConfigureSideMenu(UIViewController viewController, SidebarController sidebarController)
        {
            var mvxSideMenuSettings = viewController as IMvxSidebarMenu;

            if (mvxSideMenuSettings != null)
            {
                sidebarController.DarkOverlayAlpha = mvxSideMenuSettings.DarkOverlayAlpha;
                sidebarController.HasDarkOverlay = mvxSideMenuSettings.HasDarkOverlay;
                sidebarController.HasShadowing = mvxSideMenuSettings.HasShadowing;
                sidebarController.DisablePanGesture = mvxSideMenuSettings.DisablePanGesture;
                sidebarController.ReopenOnRotate = mvxSideMenuSettings.ReopenOnRotate;
                sidebarController.StateChangeHandler += (object sender, bool e) =>
                {
                    sidebarController.MenuWidth = mvxSideMenuSettings.MenuWidth;
                    sidebarController.ViewWillAppear(mvxSideMenuSettings.AnimateMenu);
                };
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (!_isInitialized && !_isInitializing)
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


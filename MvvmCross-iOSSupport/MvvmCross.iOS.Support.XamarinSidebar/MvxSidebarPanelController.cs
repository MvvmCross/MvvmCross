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
		public bool HasLeftMenu => LeftSidebarController != null && LeftSidebarController.MenuAreaController != null;
		public bool HasRightMenu => RightSidebarController != null && RightSidebarController.MenuAreaController != null;

        protected virtual void SetupSideMenu()
        {
			var leftSideMenu = ResolveSideMenu(MvxPanelEnum.Left);
			var rightSideMenu = ResolveSideMenu(MvxPanelEnum.Right);

			if (leftSideMenu == null && rightSideMenu == null)
			{	
				Mvx.Trace(MvxTraceLevel.Warning, $"No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'.");
				AttachNavigationController();
				return;
			}

            if (leftSideMenu != null && rightSideMenu != null)
            {
                LeftSidebarController = new SidebarController(_subRootViewController, NavigationController, leftSideMenu);
                ConfigureSideMenu(LeftSidebarController);

                RightSidebarController = new SidebarController(this, _subRootViewController, rightSideMenu);
                ConfigureSideMenu(RightSidebarController);
            }
            else if (leftSideMenu != null)
            {
                LeftSidebarController = new SidebarController(this, NavigationController, leftSideMenu);
                RightSidebarController = null;
                ConfigureSideMenu(LeftSidebarController);
            }
            else if (rightSideMenu != null)
            {
                LeftSidebarController = null;
                RightSidebarController = new SidebarController(this, NavigationController, rightSideMenu);
                ConfigureSideMenu(RightSidebarController);
            }
        }

		protected virtual void AttachNavigationController()
		{
			this.AddChildViewController(NavigationController);
			this.View.AddSubview(NavigationController.View);
		}

        protected virtual UIViewController ResolveSideMenu(MvxPanelEnum location)
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

        protected virtual IMvxIosView CreateInstance(Type viewControllerType)
        {
            var viewModelType = GetBaseType(viewControllerType);
            var presenter = Mvx.Resolve<IMvxIosViewPresenter>();
            return presenter.CreateViewControllerFor(new MvxViewModelRequest(viewModelType, null, null, null));
        }

        protected virtual Type GetBaseType(Type type)
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

        protected virtual void ConfigureSideMenu(SidebarController sidebarController)
        {
            var mvxSideMenuSettings = sidebarController.MenuAreaController as IMvxSidebarMenu;

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

			SetupSideMenu();
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

        protected virtual void OpenMenu(SidebarController sidebarController)
        {
            if (sidebarController != null && !sidebarController.IsOpen)
                sidebarController.OpenMenu();
        }
    }
}


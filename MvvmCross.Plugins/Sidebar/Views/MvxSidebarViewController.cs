// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using SidebarNavigation;
using UIKit;

namespace MvvmCross.Plugin.Sidebar.Views
{
    public class MvxSidebarViewController : UIViewController, IMvxSidebarViewController
    {
        private readonly UIViewController _subRootViewController;

        private bool _menuSetupSet;

        public MvxSidebarViewController()
        {
            _subRootViewController = new UIViewController();
        }

        public void SetNavigationController(UINavigationController navigationController)
        {
            NavigationController = navigationController;
        }

        public bool StatusBarHidden { get; set; }

        public bool ToggleStatusBarHiddenOnOpen { get; set; } = false;

        public new UINavigationController NavigationController { get; private set; }

        public SidebarController LeftSidebarController { get; private set; }

        public SidebarController RightSidebarController { get; private set; }

        public bool HasLeftMenu
        {
            get
            {
                if (!_menuSetupSet)
                {
                    SetupSideMenu();
                    _menuSetupSet = true;
                }
                return LeftSidebarController != null && LeftSidebarController.MenuAreaController != null;
            }
        }

        public bool HasRightMenu
        {
            get
            {
                if (!_menuSetupSet)
                {
                    SetupSideMenu();
                    _menuSetupSet = true;
                }
                return RightSidebarController != null && RightSidebarController.MenuAreaController != null;
            }
        }

        protected virtual void SetupSideMenu()
        {
            var leftSideMenu = ResolveSideMenu(MvxPanelEnum.Left);
            var rightSideMenu = ResolveSideMenu(MvxPanelEnum.Right);

            if (leftSideMenu == null && rightSideMenu == null)
            {
                MvxPluginLog.Instance.Warn($"No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'.");
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
            AddChildViewController(NavigationController);
            View.AddSubview(NavigationController.View);
        }

        protected virtual UIViewController ResolveSideMenu(MvxPanelEnum location)
        {
            var assembly = Assembly.GetEntryAssembly();

            var types = (from type in assembly.GetTypes()
                         from attribute in type.GetCustomAttributes<MvxSidebarPresentationAttribute>(true)
                         where attribute.Panel == location
                         select type).ToArray();

            if (types == null || types.Length == 0)
            {
                return null;
            }

            if (types != null && types.Length > 1)
            {
                MvxPluginLog.Instance.Warn($"Found more then one {location.ToString()} panel, using the first one in the array ({types[0].ToString()}).");
            }

            return CreateInstance(types[0]) as UIViewController;
        }

        protected virtual IMvxIosView CreateInstance(Type viewControllerType)
        {
            var viewModelType = GetBaseType(viewControllerType);
            var presenter = Mvx.IoCProvider.Resolve<IMvxIosViewPresenter>();
            return presenter.CreateViewControllerFor(new MvxViewModelRequest(viewModelType, null, null));
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
            if (sidebarController.MenuAreaController is IMvxSidebarMenu mvxSideMenuSettings)
            {
                sidebarController.DarkOverlayAlpha = mvxSideMenuSettings.DarkOverlayAlpha;
                sidebarController.HasDarkOverlay = mvxSideMenuSettings.HasDarkOverlay;
                sidebarController.HasShadowing = mvxSideMenuSettings.HasShadowing;
                sidebarController.ShadowColor = mvxSideMenuSettings.ShadowColor;
                sidebarController.ShadowRadius = mvxSideMenuSettings.ShadowRadius;
                sidebarController.ShadowOpacity = mvxSideMenuSettings.ShadowOpacity;
                sidebarController.DisablePanGesture = mvxSideMenuSettings.DisablePanGesture;
                sidebarController.ReopenOnRotate = mvxSideMenuSettings.ReopenOnRotate;
                sidebarController.StateChangeHandler += (object sender, bool isOpen) =>
                {
                    sidebarController.MenuWidth = mvxSideMenuSettings.MenuWidth;
                    sidebarController.ViewWillAppear(mvxSideMenuSettings.AnimateMenu);

                    if (isOpen)
                    {
                        mvxSideMenuSettings.MenuDidOpen();
                    }
                    else
                    {
                        mvxSideMenuSettings.MenuDidClose();
                    }
                };
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
                () =>
                {
                    StatusBarHidden = !StatusBarHidden;
                    SetNeedsStatusBarAppearanceUpdate();
                }
            );
        }

        public void CloseMenu()
        {
            CloseMenu(LeftSidebarController);
            CloseMenu(RightSidebarController);
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
            {
                var sidebarMenu = sidebarController.MenuAreaController as IMvxSidebarMenu;
                sidebarMenu?.MenuWillOpen();
                sidebarController.OpenMenu();
            }
        }

        protected virtual void CloseMenu(SidebarController sidebarController)
        {
            if (sidebarController != null && sidebarController.IsOpen)
            {
                var sidebarMenu = sidebarController.MenuAreaController as IMvxSidebarMenu;
                sidebarMenu?.MenuWillClose();
                sidebarController.CloseMenu();
            }
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (NavigationController.ViewControllers.Count() > 1)
            {
                NavigationController.PopViewController(true);
                return true;
            }
            return false;
        }
    }
}

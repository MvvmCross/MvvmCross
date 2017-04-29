﻿using System;
using System.Linq;
using System.Reflection;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using SidebarNavigation;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Views
{
    public class MvxSidebarViewController : UIViewController, IMvxSidebarViewController
    {
        private readonly UIViewController _subRootViewController;

        public MvxSidebarViewController()
        {
            _subRootViewController = new UIViewController();
        }

        public bool StatusBarHidden { get; set; }
        public bool ToggleStatusBarHiddenOnOpen { get; set; } = false;
        public new UINavigationController NavigationController { get; private set; }
        public SidebarController LeftSidebarController { get; private set; }
        public SidebarController RightSidebarController { get; private set; }
        public bool HasLeftMenu => LeftSidebarController != null && LeftSidebarController.MenuAreaController != null;
        public bool HasRightMenu => RightSidebarController != null && RightSidebarController.MenuAreaController != null;

        public override UIStatusBarAnimation PreferredStatusBarUpdateAnimation => UIStatusBarAnimation.Fade;

        public void SetNavigationController(UINavigationController navigationController)
        {
            NavigationController = navigationController;
        }

        public void CloseMenu()
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

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (NavigationController.ViewControllers.Count() > 1)
            {
                NavigationController.PopViewController(true);
                return true;
            }
            return false;
        }

        protected virtual void SetupSideMenu()
        {
            var leftSideMenu = ResolveSideMenu(MvxPanelEnum.Left);
            var rightSideMenu = ResolveSideMenu(MvxPanelEnum.Right);

            if (leftSideMenu == null && rightSideMenu == null)
            {
                Mvx.Trace(MvxTraceLevel.Warning,
                    $"No sidemenu found. To use a sidemenu decorate the viewcontroller class with the 'MvxPanelPresentationAttribute' class and set the panel to 'Left' or 'Right'.");
                AttachNavigationController();
                return;
            }

            if (leftSideMenu != null && rightSideMenu != null)
            {
                LeftSidebarController =
                    new SidebarController(_subRootViewController, NavigationController, leftSideMenu);
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
                return null;

            if (types != null && types.Length > 1)
                Mvx.Trace(MvxTraceLevel.Warning,
                    $"Found more then one {location} panel, using the first one in the array ({types[0]}).");

            return CreateInstance(types[0]) as UIViewController;
        }

        protected virtual IMvxIosView CreateInstance(Type viewControllerType)
        {
            var viewModelType = GetBaseType(viewControllerType);
            var presenter = Mvx.Resolve<IMvxIosViewPresenter>();
            return presenter.CreateViewControllerFor(new MvxViewModelRequest(viewModelType, null, null));
        }

        protected virtual Type GetBaseType(Type type)
        {
            while (type.BaseType != null)
            {
                type = type.BaseType;
                if (type.IsGenericType)
                {
                    var viewModelType = type.GetGenericArguments()
                        .FirstOrDefault(argument => typeof(IMvxViewModel).IsAssignableFrom(argument));

                    if (viewModelType != null)
                        return viewModelType;
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
                sidebarController.StateChangeHandler += (sender, e) =>
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

        protected virtual void OpenMenu(SidebarController sidebarController)
        {
            if (sidebarController != null && !sidebarController.IsOpen)
                sidebarController.OpenMenu();
        }
    }
}
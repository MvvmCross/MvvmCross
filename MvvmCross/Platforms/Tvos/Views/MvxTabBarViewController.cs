// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platforms.Tvos.Presenters;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxTabBarViewController
        : MvxBaseTabBarViewController, IMvxTabBarViewController
    {
        private int _tabsCount = 0;

        protected MvxTabBarViewController()
            : base()
        {
        }

        protected MvxTabBarViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                if (Mvx.IoCProvider.TryResolve(out IMvxTvosViewPresenter iPresenter)
                    && iPresenter is MvxTvosViewPresenter mvxTvosViewPresenter)
                {
                    mvxTvosViewPresenter.CloseTabBarViewController();
                }
            }
        }

        public virtual void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute)
        {
            if (!string.IsNullOrEmpty(attribute.TabAccessibilityIdentifier))
                viewController.View.AccessibilityIdentifier = attribute.TabAccessibilityIdentifier;

            // setup Tab
            SetTitleAndTabBarItem(viewController, attribute);

            // add Tab
            var currentTabs = new List<UIViewController>();
            if (ViewControllers != null)
            {
                currentTabs = ViewControllers.ToList();
            }

            currentTabs.Add(viewController);

            // update current Tabs
            ViewControllers = currentTabs.ToArray();
        }

        protected virtual void SetTitleAndTabBarItem(UIViewController viewController, MvxTabPresentationAttribute attribute)
        {
            _tabsCount++;

            viewController.Title = attribute.TabName;

            if (!string.IsNullOrEmpty(attribute.TabIconName))
                viewController.TabBarItem = new UITabBarItem(attribute.TabName, UIImage.FromBundle(attribute.TabIconName), _tabsCount);

            if (!string.IsNullOrEmpty(attribute.TabSelectedIconName))
                viewController.TabBarItem.SelectedImage = UIImage.FromBundle(attribute.TabSelectedIconName);
        }

        public virtual bool ShowChildView(UIViewController viewController)
        {
            var navigationController = SelectedViewController as UINavigationController;

            // if the current selected ViewController is not a NavigationController, then a child cannot be shown
            if (navigationController == null)
            {
                return false;
            }

            navigationController.PushViewController(viewController, true);
            return true;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (SelectedViewController is UINavigationController navController
                && navController.ViewControllers != null
                && navController.ViewControllers.Any())
            {
                // if the ViewModel to close if the last in the stack, close it animated
                if (navController.TopViewController.GetIMvxTvosView().ViewModel == viewModel)
                {
                    navController.PopViewController(true);
                    return true;
                }

                var controllers = navController.ViewControllers.ToList();
                var controllerToClose = controllers.FirstOrDefault(vc => vc.GetIMvxTvosView().ViewModel == viewModel);

                if (controllerToClose != null)
                {
                    controllers.Remove(controllerToClose);
                    navController.ViewControllers = controllers.ToArray();

                    return true;
                }
            }

            return false;
        }

        public virtual bool CloseTabViewModel(IMvxViewModel viewModel)
        {
            if (ViewControllers == null || !ViewControllers.Any())
                return false;

            // loop through plain Tabs
            var plainToClose = ViewControllers.Where(v => !(v is UINavigationController))
                                              .Select(v => v.GetIMvxTvosView())
                                              .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
            if (plainToClose != null)
            {
                RemoveTabController((UIViewController)plainToClose);
                return true;
            }

            // loop through nav stack Tabs
            UIViewController toClose = null;
            foreach (var vc in ViewControllers.Where(v => v is UINavigationController))
            {
                var root = ((UINavigationController)vc).ViewControllers.FirstOrDefault();
                var vcFromRoot = root.GetIMvxTvosView();
                if (root != null && vcFromRoot.ViewModel == viewModel)
                {
                    toClose = root;
                    break;
                }
            }
            if (toClose != null)
            {
                RemoveTabController((UIViewController)toClose);
                return true;
            }

            return false;
        }

        public void PresentViewControllerWithNavigation(UIViewController controller,
                                                        bool animated = true,
                                                        Action completionHandler = null)
        {
            PresentViewController(new UINavigationController(controller), animated, completionHandler);
        }

        public virtual bool CanShowChildView()
        {
            return SelectedViewController is UINavigationController;
        }

        protected virtual void RemoveTabController(UIViewController toClose)
        {
            var newTabs = ViewControllers.Where(v => v != toClose);
            ViewControllers = newTabs.ToArray();
        }
    }

    public class MvxTabBarViewController<TViewModel> : MvxTabBarViewController
        where TViewModel : IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public virtual UIViewController VisibleUIViewController
        {
            get
            {
                var topViewController = (SelectedViewController as UINavigationController)
                    ?.TopViewController ?? SelectedViewController;

                if (topViewController.PresentedViewController != null)
                {
                    var presentedNavigationController = topViewController.PresentedViewController as UINavigationController;
                    if (presentedNavigationController != null)
                    {
                        return presentedNavigationController.TopViewController;
                    }
                    else
                    {
                        return topViewController.PresentedViewController;
                    }
                }
                else
                {
                    return topViewController;
                }
            }
        }

        protected MvxTabBarViewController()
        {
        }

        protected MvxTabBarViewController(IntPtr handle)
            : base(handle)
        {
        }
    }
}

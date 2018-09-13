// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxTabBarViewController : MvxBaseTabBarViewController, IMvxTabBarViewController
    {
        private int _tabsCount = 0;

        public virtual UIViewController VisibleUIViewController
        {
            get
            {
                var topViewController = (SelectedViewController as UINavigationController)?.TopViewController ?? SelectedViewController;

                if (topViewController != null && topViewController.PresentedViewController != null)
                {
                    if (topViewController.PresentedViewController is UINavigationController presentedNavigationController)
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

        public MvxTabBarViewController() : base()
        {
            // WORKAROUND: UIKit makes a first ViewDidLoad call, because a TabViewController expects it's view (tabs) to be drawn 
            // on construction. Therefore we need to call ViewDidLoad "manually", otherwise ViewModel will be null
            ViewDidLoad();
        }

        public MvxTabBarViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (IsMovingFromParentViewController)
            {
                if (Mvx.IoCProvider.TryResolve(out IMvxIosViewPresenter iPresenter)
                    && iPresenter is MvxIosViewPresenter mvxIosViewPresenter)
                {
                    mvxIosViewPresenter.CloseTabBarViewController();
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
            if (SelectedIndex > 5) // when more menu item is currently visible, selected index has value higher than 5
            {
                MoreNavigationController.PushViewController(viewController, true);
                return true;
            }

            var navigationController = SelectedViewController as UINavigationController;

            // if the current selected ViewController is not a NavigationController, then a child cannot be shown
            if (navigationController == null)
            {
                return false;
            }

            navigationController.PushViewController(viewController, true);

            return true;
        }

        public virtual bool CanShowChildView()
        {
            return SelectedViewController is UINavigationController;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (SelectedIndex > 5 && (MoreNavigationController?.ViewControllers?.Any() ?? false))
            {
                var lastViewController = (MoreNavigationController.ViewControllers.Last()).GetIMvxIosView();

                if (lastViewController != null && lastViewController.ViewModel == viewModel)
                {
                    MoreNavigationController.PopViewController(true);
                    return true;
                }
            }

            if (SelectedViewController is UINavigationController navController
                && navController.ViewControllers != null
                && navController.ViewControllers.Any())
            {
                // if the ViewModel to close if the last in the stack, close it animated
                if (navController.TopViewController.GetIMvxIosView().ViewModel == viewModel)
                {
                    navController.PopViewController(true);
                    return true;
                }

                var controllers = navController.ViewControllers.ToList();
                var controllerToClose = controllers.FirstOrDefault(vc => vc.GetIMvxIosView().ViewModel == viewModel);

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
                                              .Select(v => v.GetIMvxIosView())
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
                if (root != null && root.GetIMvxIosView().ViewModel == viewModel)
                {
                    toClose = vc;
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

        public void PresentViewControllerWithNavigation(UIViewController controller, bool animated = true, Action completionHandler = null)
        {
            PresentViewController(new UINavigationController(controller), animated, completionHandler);
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

        public MvxTabBarViewController() : base()
        {
        }

        public MvxTabBarViewController(IntPtr handle) : base(handle)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public class MvxTabBarViewController : MvxBaseTabBarViewController, IMvxTabBarViewController
    {
        private int _tabsCount = 0;

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
                var presenter = Mvx.Resolve<IMvxIosViewPresenter>() as MvxIosViewPresenter;
                presenter.CloseTabBarViewController();
            }
        }

        //keep changes for selected icon from breaking current release
        public virtual void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null)
        {
            ShowTabView(viewController, tabTitle, tabIconName, null, tabAccessibilityIdentifier);
        }

        public virtual void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabSelectedIconName = null, string tabAccessibilityIdentifier = null)
        {
            if (!string.IsNullOrEmpty(tabAccessibilityIdentifier))
                viewController.View.AccessibilityIdentifier = tabAccessibilityIdentifier;

            // setup Tab
            SetTitleAndTabBarItem(viewController, tabTitle, tabIconName, tabSelectedIconName);

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

        //keep changes for selected icon from breaking current release
        protected virtual void SetTitleAndTabBarItem(UIViewController viewController, string title, string iconName)
        {
            _tabsCount++;

            viewController.Title = title;

            if (!string.IsNullOrEmpty(iconName))
                viewController.TabBarItem = new UITabBarItem(title, UIImage.FromBundle(iconName), _tabsCount);
        }

        protected virtual void SetTitleAndTabBarItem(UIViewController viewController, string title, string iconName, string selectedIconName)
        {
            SetTitleAndTabBarItem(viewController, title, iconName);

            if (!string.IsNullOrEmpty(selectedIconName))
                viewController.TabBarItem.SelectedImage = UIImage.FromBundle(selectedIconName);
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

        public virtual bool CanShowChildView(UIViewController viewController)
        {
            return SelectedViewController is UINavigationController;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            var navController = SelectedViewController as UINavigationController;
            if (navController != null)
            {
                var root = navController.ViewControllers.FirstOrDefault();

                // check if the ViewModel to close is the Root of the Navigation Stack, 
                // otherwise pop the last in stack
                if (root != null
                   && root is IMvxIosView iosView
                   && iosView.ViewModel == viewModel)
                {
                    RemoveTabController(navController);
                }
                else
                {
                    navController.PopViewController(true);
                }

                return true;
            }

            // loop through Tabs
            var toClose = ViewControllers.Where(v => v.GetType() != typeof(MvxNavigationController))
                                         .Select(v => v.GetIMvxIosView())
                                         .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
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

        public virtual UIViewController VisibleUIViewController
        {
            get
            {
                var topViewController = (SelectedViewController as UINavigationController)?.TopViewController ?? SelectedViewController;

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

        public MvxTabBarViewController() : base()
        {
        }

        public MvxTabBarViewController(IntPtr handle) : base(handle)
        {
        }
    }
}

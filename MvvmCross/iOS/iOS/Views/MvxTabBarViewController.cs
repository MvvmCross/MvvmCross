using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
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

        public virtual void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null)
        {
            if(!string.IsNullOrEmpty(tabAccessibilityIdentifier))
                viewController.View.AccessibilityIdentifier = tabAccessibilityIdentifier;

            // setup Tab
            SetTitleAndTabBarItem(viewController, tabTitle, tabIconName);

            // add Tab
            var currentTabs = new List<UIViewController>();
            if(ViewControllers != null)
            {
                currentTabs = ViewControllers.ToList();
            }

            if(viewController as UINavigationController != null)
                (viewController as UINavigationController).NavigationBar.Translucent = false;

            currentTabs.Add(viewController);

            // update current Tabs
            ViewControllers = currentTabs.ToArray();
        }

        protected virtual void SetTitleAndTabBarItem(UIViewController viewController, string title, string iconName)
        {
            viewController.Title = title;

            if(!string.IsNullOrEmpty(iconName))
                viewController.TabBarItem = new UITabBarItem(title, UIImage.FromBundle(iconName), this._tabsCount);

            _tabsCount++;
        }

        public virtual bool ShowChildView(UIViewController viewController)
        {
            var navigationController = (UINavigationController)SelectedViewController;

            // if the current selected ViewController is not a NavigationController, then a child cannot be shown
            if(navigationController == null)
            {
                return false;
            }

            navigationController.PushViewController(viewController, true);

            return true;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            // current implementation assumes the ViewModel to close is the currently shown ViewController 
            var navController = SelectedViewController as UINavigationController;
            if(navController != null)
            {
                navController.PopViewController(true);
                return true;
            }
            return false;
        }

        public void PresentViewControllerWithNavigation(UIViewController controller, bool animated = true, Action completionHandler = null)
        {
            PresentViewController(new UINavigationController(controller), animated, completionHandler);
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
                var topViewController = (SelectedViewController as UINavigationController).TopViewController;
                if(topViewController.PresentedViewController != null)
                {
                    var presentedNavigationController = (topViewController.PresentedViewController as UINavigationController);
                    if(presentedNavigationController != null)
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

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.iOS.Views.Presenters.Attributes;
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
                if (Mvx.Resolve<IMvxIosViewPresenter>() is MvxIosViewPresenter presenter)
                {
                    presenter.CloseTabBarViewController();
                };
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

        public virtual bool CanShowChildView()
        {
            return SelectedViewController is UINavigationController;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
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
            var toClose = ViewControllers.Where(v => v is UINavigationController)
                                         .Select(v => ((UINavigationController)v).ViewControllers.FirstOrDefault())
                                         .Where(vc => vc != null)
                                         .Select(vc => vc.GetIMvxIosView())
                                         .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
            if (toClose != null)
            {
                RemoveTabController((UIViewController)plainToClose);
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

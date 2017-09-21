using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Core.Views;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters
{
    public class MvxIosViewPresenter : MvxBaseIosViewPresenter
    {
        protected readonly IUIApplicationDelegate _applicationDelegate;
        protected readonly UIWindow _window;
        private Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;
        protected Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> AttributeTypesToShowMethodDictionary
        {
            get
            {
                if (_attributeTypesToShowMethodDictionary == null)
                {
                    _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesToShowMethodDictionary;
            }
        }

        public UINavigationController MasterNavigationController { get; protected set; }

        public List<UIViewController> ModalViewControllers { get; protected set; } = new List<UIViewController>();

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public IMvxSplitViewController SplitViewController { get; protected set; }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        protected virtual void RegisterAttributeTypes()
        {
            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxRootPresentationAttribute),
                (vc, attribute, request) => ShowRootViewController(vc, (MvxRootPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxChildPresentationAttribute),
                (vc, attribute, request) => ShowChildViewController(vc, (MvxChildPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                (vc, attribute, request) => ShowTabViewController(vc, (MvxTabPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                (vc, attribute, request) => ShowModalViewController(vc, (MvxModalPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxMasterSplitViewPresentationAttribute),
                (vc, attribute, request) => ShowMasterSplitViewController(vc, (MvxMasterSplitViewPresentationAttribute)attribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxDetailSplitViewPresentationAttribute),
                (vc, attribute, request) => ShowDetailSplitViewController(vc, (MvxDetailSplitViewPresentationAttribute)attribute, request));
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
            }
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view, request);
        }

        public virtual void Show(IMvxIosView view, MvxViewModelRequest request)
        {
            var viewController = view as UIViewController;
            var attribute = GetPresentationAttributes(viewController);
            var attributeType = attribute.GetType();

            if (AttributeTypesToShowMethodDictionary.TryGetValue(attributeType,
                out Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction))
            {
                showAction.Invoke(viewController, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual void ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // check if viewController is a TabBarController
            if (viewController is IMvxTabBarViewController tabBarController)
            {
                TabBarViewController = tabBarController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                CleanupModalViewControllers();
                CloseSplitViewController();

                return;
            }

            // check if viewController is a SplitViewController
            if (viewController is IMvxSplitViewController splitController)
            {
                SplitViewController = splitController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                CleanupModalViewControllers();
                CloseTabBarViewController();

                return;
            }

            // set root initiating stack navigation or just a plain controller
            SetupWindowRootNavigation(viewController, attribute);

            CleanupModalViewControllers();
            CloseTabBarViewController();
            CloseSplitViewController();
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            if (attribute.WrapInNavigationController)
            {
                MasterNavigationController = CreateNavigationController(viewController);

                SetWindowRootViewController(MasterNavigationController);
            }
            else
            {
                SetWindowRootViewController(viewController);

                CloseMasterNavigationController();
            }
        }

        protected virtual void ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if (ModalViewControllers.Any())
            {
                if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
                {
                    PushViewControllerIntoStack(modalNavController, viewController, attribute.Animated);

                    return;
                }
                else
                {
                    throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
                }
            }

            if (TabBarViewController != null && TabBarViewController.ShowChildView(viewController))
            {
                return;
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute.Animated);
                return;
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        protected virtual void ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (TabBarViewController == null)
                throw new MvxException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if (viewController is IMvxTabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            TabBarViewController.ShowTabView(
                viewController,
                attribute);
        }

        protected virtual void ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;
            if (attribute.PreferredContentSize != default(CGSize))
                viewController.PreferredContentSize = attribute.PreferredContentSize;

            // Check if there is a modal already presented first. Otherwise use the window root
            var modalHost = ModalViewControllers.LastOrDefault() ?? _window.RootViewController;

            modalHost.PresentViewController(
                viewController,
                attribute.Animated,
                null);

            ModalViewControllers.Add(viewController);
        }

        protected virtual void ShowMasterSplitViewController(
            UIViewController viewController,
            MvxMasterSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SplitViewController == null)
                throw new MvxException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute.WrapInNavigationController);
        }

        protected virtual void ShowDetailSplitViewController(
            UIViewController viewController,
            MvxDetailSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SplitViewController == null)
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute.WrapInNavigationController);
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
            return true;
        }

        public override void Close(IMvxViewModel toClose)
        {
            // check if there is a modal presented
            if (ModalViewControllers.Any() && CloseModalViewController(toClose))
                return;

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if (TabBarViewController != null && TabBarViewController.CloseChildViewModel(toClose))
                return;

            // if the current root is a SplitViewController, delegate close responsibility to it
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(toClose))
                return;

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, toClose))
                return;

            MvxTrace.Warning($"Could not close ViewModel type {toClose.GetType().Name}");
        }

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected virtual void PushViewControllerIntoStack(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            navigationController.PushViewController(viewController, animated);

            if (viewController is IMvxTabBarViewController)
                TabBarViewController = viewController as IMvxTabBarViewController;
        }

        protected virtual bool CloseModalViewController(IMvxViewModel toClose)
        {
            // check if there is a modal stack presented
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                if (TryCloseViewControllerInsideStack(modalNavController, toClose))
                {
                    // First() is the RootViewController of the stack. If it is being closed, then remove the nav stack
                    if (modalNavController.ViewControllers.First().GetIMvxIosView().ViewModel == toClose)
                    {
                        CloseModalViewController(modalNavController);
                    }
                    return true;
                }
            }
            else
            {
                // close any plain modal presented
                var last = ModalViewControllers.Last();
                if (last.GetIMvxIosView().ViewModel == toClose)
                {
                    CloseModalViewController(last);
                    return true;
                }
            }

            return false;
        }

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, IMvxViewModel toClose)
        {
            // check for top view controller
            var topView = navController.TopViewController.GetIMvxIosView();
            if (topView != null && topView.ViewModel == toClose)
            {
                navController.PopViewController(true);
                return true;
            }

            // loop through stack
            foreach (var viewController in navController.ViewControllers)
            {
                var mvxView = viewController.GetIMvxIosView();
                if (mvxView.ViewModel == toClose)
                {
                    var newViewControllers = navController.ViewControllers.Where(v => v != viewController).ToArray();
                    navController.ViewControllers = newViewControllers;
                    return true;
                }
            }
            return false;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            CloseModalViewController(ModalViewControllers.Last());
        }

        protected void CloseMasterNavigationController()
        {
            if (MasterNavigationController == null)
                return;

            if (MasterNavigationController.ViewControllers != null)
            {
                foreach (var item in MasterNavigationController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            MasterNavigationController = null;
        }

        protected virtual void CloseModalViewController(UIViewController modalController)
        {
            if (modalController == null)
                return;

            if (modalController is UINavigationController modalNavController)
            {
                foreach (var item in modalNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            modalController.DismissViewController(true, null);
            ModalViewControllers.Remove(modalController);
        }

        protected void CleanupModalViewControllers()
        {
            while (ModalViewControllers.Any())
            {
                CloseModalViewController(ModalViewControllers.LastOrDefault());
            }
        }

        public void CloseTabBarViewController()
        {
            if (TabBarViewController == null)
                return;

            if (TabBarViewController is UITabBarController tabsController
                && tabsController.ViewControllers != null)
            {
                foreach (var item in tabsController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            TabBarViewController = null;
        }

        protected void CloseSplitViewController()
        {
            if (SplitViewController == null)
                return;

            if (SplitViewController is UISplitViewController splitController
               && splitController.ViewControllers != null)
            {
                foreach (var item in splitController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            SplitViewController = null;
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            foreach (var v in _window.Subviews)
                v.RemoveFromSuperview();

            _window.RootViewController = controller;
        }

        protected virtual MvxBasePresentationAttribute GetPresentationAttributes(UIViewController viewController)
        {
            if (viewController is IMvxOverridePresentationAttribute vc)
            {
                var presentationAttribute = vc.PresentationAttribute();

                if (presentationAttribute != null)
                    return presentationAttribute;
            }

            var attribute = viewController.GetType()
                .GetCustomAttributes(typeof(MvxBasePresentationAttribute), true)
                .FirstOrDefault() as MvxBasePresentationAttribute;
            if (attribute != null)
            {
                return attribute;
            }

            if (MasterNavigationController == null &&
               (TabBarViewController == null ||
               !TabBarViewController.CanShowChildView(viewController)))
            {
                MvxTrace.Trace($"PresentationAttribute nor MasterNavigationController found for {viewController.GetType().Name}. " +
                    $"Assuming Root presentation");
                return new MvxRootPresentationAttribute() { WrapInNavigationController = true };
            }

            MvxTrace.Trace($"PresentationAttribute not found for {viewController.GetType().Name}. " +
                $"Assuming animated Child presentation");
            return new MvxChildPresentationAttribute();
        }
    }
}

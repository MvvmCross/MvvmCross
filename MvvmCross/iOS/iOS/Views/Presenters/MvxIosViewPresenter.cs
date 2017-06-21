using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters
{
    public class MvxIosViewPresenter : MvxBaseIosViewPresenter
    {
        protected readonly IUIApplicationDelegate _applicationDelegate;
        protected readonly UIWindow _window;
        protected Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> 
            _attributeTypesToShowMethodDictionary;

        public UINavigationController MasterNavigationController { get; protected set; }

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public IMvxSplitViewController SplitViewController { get; protected set; }

        protected UIViewController TopViewController { get; private set; }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;

            _attributeTypesToShowMethodDictionary = 
                new Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>();

            RegisterAttributeTypes();
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

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected void SetTopViewController(UIViewController viewController)
        {
            if (viewController == null)
                return;

            // clear view controllers
            MasterNavigationController = null;
            TabBarViewController = null;
            SplitViewController = null;

            // check if viewController is a TabBarController
            if (viewController is IMvxTabBarViewController tabBarVc)
                TabBarViewController = tabBarVc;

            // check if viewController is a SplitViewController
            if (viewController is IMvxSplitViewController splitVc)
                SplitViewController = splitVc;

            // check if viewController is trying to initialize a navigation stack
            if (viewController is UINavigationController navVc)
                MasterNavigationController = navVc;

            // Control of the top modal view controller
            // Because the modal view can be a UIviewController
            TopViewController = viewController;
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

            if (_attributeTypesToShowMethodDictionary.TryGetValue(attributeType, 
                out Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction))
                showAction.Invoke(viewController, attribute, request);

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual void ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // check navigation controller
            viewController = attribute.WrapInNavigationController ? CreateNavigationController(viewController) : viewController;
            SetTopViewController(viewController);
            SetWindowRootViewController(viewController);
        }

        protected virtual void ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if (TabBarViewController != null && TabBarViewController.ShowChildView(viewController))
            {
                return;
            }

            if (MasterNavigationController != null)
            {
                MasterNavigationController.PushViewController(viewController, attribute.Animated);

                if (viewController is IMvxTabBarViewController)
                    SetTopViewController(viewController);

                return;
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current Root!");
        }

        protected virtual void ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (TabBarViewController == null)
                throw new MvxException("Trying to show a tab without a TabBarViewController, this is not possible!");

            string tabName = attribute.TabName;
            string tabIconName = attribute.TabIconName;
            if (viewController is IMvxTabBarItemViewController tabBarItem)
            {
                tabName = tabBarItem.TabName;
                tabIconName = tabBarItem.TabIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            TabBarViewController.ShowTabView(
                viewController,
                tabName,
                tabIconName,
                attribute.TabAccessibilityIdentifier);
        }

        protected virtual void ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;
            if (attribute.PreferredContentSize != default(CGSize))
                viewController.PreferredContentSize = attribute.PreferredContentSize;

            TopViewController?.PresentViewController(viewController, attribute.Animated, null);
            SetTopViewController(viewController);
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
            if (CloseModalViewController(toClose))
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

        protected virtual bool CloseModalViewController(IMvxViewModel toClose)
        {
            // try get MvxModalPresentationAttribute or presentingViewController
            var topViewController = TopViewController;
            if (!(GetPresentationAttributes(topViewController) is MvxModalPresentationAttribute) || topViewController?.PresentingViewController == null)
            {
                return false;
            }

            var presentingVc = topViewController.PresentingViewController;
            topViewController.DismissViewController(true, null);
            SetTopViewController(presentingVc);

            return true;
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
            base.NativeModalViewControllerDisappearedOnItsOwn();
            if (CloseModalViewController(null))
                return;

            if (MasterNavigationController != null)
                MasterNavigationController.PopToRootViewController(true);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            foreach (var v in _window.Subviews)
                v.RemoveFromSuperview();

            _window.AddSubview(controller.View);
            _window.RootViewController = controller;
        }

        protected MvxBasePresentationAttribute GetPresentationAttributes(UIViewController viewController)
        {
            if (viewController is IMvxOverridePresentationAttribute vc)
            {
                var presentationAttribute = vc.PresentationAttribute();

                if (presentationAttribute != null)
                    return presentationAttribute;
            }

            // first try
            if (viewController.GetType().GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).FirstOrDefault() is MvxBasePresentationAttribute attribute)
            {
                return attribute;
            }

            UIViewController childViewController;
            if (MasterNavigationController != null)
                childViewController = MasterNavigationController.TopViewController;
            else if (TabBarViewController != null)
                childViewController = (TabBarViewController as UITabBarController)?.SelectedViewController;
            else if (SplitViewController != null)
                childViewController = (SplitViewController as UISplitViewController)?.ViewControllers?.FirstOrDefault();
            else
                childViewController = null;

            // last try
            if (childViewController != null)
            {
                attribute = childViewController.GetType().GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).FirstOrDefault() as MvxBasePresentationAttribute;
                if (attribute != null)
                {
                    return attribute;
                }
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

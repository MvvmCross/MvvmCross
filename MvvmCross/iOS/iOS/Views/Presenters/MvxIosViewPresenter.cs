using System;
using System.Collections.Generic;
using System.Linq;
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
        protected Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> _attributeTypesToShowMethodDictionary;

        public UINavigationController MasterNavigationController { get; protected set; }

        public UINavigationController ModalNavigationController { get; protected set; }

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public IMvxSplitViewController SplitViewController { get; protected set; }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;

            _attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>();

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

            if(hint is MvxClosePresentationHint)
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

            Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction;
            if(!_attributeTypesToShowMethodDictionary.TryGetValue(attribute.GetType(), out showAction))
                throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

            showAction.Invoke(viewController, attribute, request);
        }

        protected virtual void ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // check if viewController is a TabBarController
            if(viewController is IMvxTabBarViewController)
            {
                TabBarViewController = viewController as IMvxTabBarViewController;
                SetWindowRootViewController(viewController);

                CloseMasterNavigationController();
                CloseModalNavigationController();
                CloseSplitViewController();

                return;
            }

            // check if viewController is a SplitViewController
            if(viewController is IMvxSplitViewController)
            {
                SplitViewController = viewController as IMvxSplitViewController;
                SetWindowRootViewController(viewController);

                CloseMasterNavigationController();
                CloseModalNavigationController();
                CloseTabBarViewController();

                return;
            }

            // check if viewController is trying to initialize a navigation stack
            if(attribute.WrapInNavigationController)
            {
                viewController = new MvxNavigationController(viewController);
                MasterNavigationController = viewController as MvxNavigationController;
                SetWindowRootViewController(viewController);

                CloseModalNavigationController();
                CloseTabBarViewController();
                CloseSplitViewController();

                return;
            }

            // last scenario: display the plain viewController as root
            SetWindowRootViewController(viewController);
        }

        protected virtual void ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if(viewController is IMvxTabBarViewController)
                throw new MvxException("A TabBarViewController cannot be presented as a child. Consider using Root instead");

            if(viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if(ModalNavigationController != null)
            {
                ModalNavigationController.PushViewController(viewController, attribute.Animated);
                return;
            }

            if(TabBarViewController != null)
            {
                TabBarViewController.ShowChildView(viewController);
                return;
            }

            if(MasterNavigationController != null)
            {
                MasterNavigationController.PushViewController(viewController, attribute.Animated);
                return;
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current Root!");
        }

        protected virtual void ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if(TabBarViewController == null)
                throw new MvxException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if(attribute.WrapInNavigationController)
                viewController = new MvxNavigationController(viewController);

            TabBarViewController.ShowTabView(
                viewController,
                attribute.TabName,
                attribute.TabIconName,
                attribute.TabAccessibilityIdentifier);
        }

        protected virtual void ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // if there is currently a modal ViewController, dismiss it forced (otherwise nothing happens when presenting)
            if(_window.RootViewController.PresentedViewController != null)
                _window.RootViewController.DismissViewController(attribute.Animated, null);

            // setup modal based on attribute
            if(attribute.WrapInNavigationController)
            {
                viewController = new MvxNavigationController(viewController);
                ModalNavigationController = viewController as MvxNavigationController;
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;

            _window.RootViewController.PresentViewController(
                viewController,
                attribute.Animated,
                null);
        }

        protected virtual void ShowMasterSplitViewController(
            UIViewController viewController,
            MvxMasterSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if(SplitViewController == null)
                throw new MvxException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute.WrapInNavigationController);
        }

        protected virtual void ShowDetailSplitViewController(
            UIViewController viewController,
            MvxDetailSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if(SplitViewController == null)
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute.WrapInNavigationController);
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
            return true;
        }

        public virtual void Close(IMvxViewModel toClose)
        {
            // check if there is a modal presented
            if(_window.RootViewController.PresentedViewController != null && CloseModalViewController(toClose))
                return;

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if(TabBarViewController != null && TabBarViewController.CloseChildViewModel(toClose))
                return;

            // if the current root is a SplitViewController, delegate close responsibility to it
            if(SplitViewController != null && SplitViewController.CloseChildViewModel(toClose))
                return;

            // if the current root is a NavigationController, close it in the stack
            if(MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, toClose))
                return;

            MvxTrace.Warning($"Could not close ViewModel type {toClose.GetType().Name}");
        }

        protected virtual bool CloseModalViewController(IMvxViewModel toClose)
        {
            // check if there is a modal stack presented
            if(ModalNavigationController != null)
            {
                if(TryCloseViewControllerInsideStack(ModalNavigationController, toClose))
                {
                    // First() is the RootViewController of the stack. If it is being closed, then remove the nav stack
                    if(ModalNavigationController.ViewControllers.First().GetIMvxIosView().ViewModel == toClose)
                    {
                        _window.RootViewController.DismissViewController(true, null);
                        CloseModalNavigationController();
                    }
                    return true;
                }
            }

            // close any plain modal presented
            _window.RootViewController.PresentedViewController.DismissViewController(true, null);
            return true;
        }

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, IMvxViewModel toClose)
        {
            // check for top view controller
            var topView = navController.TopViewController.GetIMvxIosView();
            if(topView != null && topView.ViewModel == toClose)
            {
                navController.PopViewController(true);
                return true;
            }

            // loop through stack
            foreach(var viewController in navController.ViewControllers)
            {
                var mvxView = viewController.GetIMvxIosView();
                if(mvxView.ViewModel == toClose)
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
            _window.RootViewController.DismissViewController(false, null);
            CloseModalNavigationController();
        }

        protected void CloseMasterNavigationController()
        {
            if(MasterNavigationController == null)
                return;

            foreach(var item in MasterNavigationController.ViewControllers)
                item.DidMoveToParentViewController(null);
            MasterNavigationController = null;
        }

        protected void CloseModalNavigationController()
        {
            if(ModalNavigationController == null)
                return;

            foreach(var item in ModalNavigationController.ViewControllers)
                item.DidMoveToParentViewController(null);
            ModalNavigationController = null;
        }

        protected void CloseTabBarViewController()
        {
            if(TabBarViewController == null)
                return;

            foreach(var item in (TabBarViewController as UITabBarController).ViewControllers)
                item.DidMoveToParentViewController(null);
            TabBarViewController = null;
        }

        protected void CloseSplitViewController()
        {
            if(SplitViewController == null)
                return;

            foreach(var item in (SplitViewController as UISplitViewController).ViewControllers)
                item.DidMoveToParentViewController(null);
            SplitViewController = null;
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            foreach(var v in _window.Subviews)
                v.RemoveFromSuperview();

            _window.AddSubview(controller.View);
            _window.RootViewController = controller;
        }

        protected MvxBasePresentationAttribute GetPresentationAttributes(UIViewController viewController)
        {
            var attributes = viewController.GetType().GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).FirstOrDefault() as MvxBasePresentationAttribute;
            if(attributes == null)
            {
                MvxTrace.Trace($"PresentationAttribute not found for {viewController.GetType().Name}, assuming animated Child presentation");
                attributes = new MvxChildPresentationAttribute();
            }
            return attributes;
        }
    }
}

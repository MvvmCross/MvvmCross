using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Platform.Exceptions;
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
                (vc, attribute, request) => ShowRootViewController(vc, attribute as MvxRootPresentationAttribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxChildPresentationAttribute),
                (vc, attribute, request) => ShowChildViewController(vc, attribute as MvxChildPresentationAttribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                (vc, attribute, request) => ShowTabViewController(vc, attribute as MvxTabPresentationAttribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                (vc, attribute, request) => ShowModalViewController(vc, attribute as MvxModalPresentationAttribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxMasterSplitViewPresentationAttribute),
                (vc, attribute, request) => ShowMasterSplitViewController(vc, attribute as MvxMasterSplitViewPresentationAttribute, request));

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxDetailSplitViewPresentationAttribute),
                (vc, attribute, request) => ShowDetailSplitViewController(vc, attribute as MvxDetailSplitViewPresentationAttribute, request));
        }

        //protected virtual UIViewController CurrentTopViewController
        //{
        //    get
        //    {
        //        // exploring the stack manually instead checking MasterNavigationController / ModalNavigationController
        //        // allows the user to present his own modals outside the presenter without problem

        //        var windowRoot = _window.RootViewController;

        //        // check for a presented ViewController
        //        if(windowRoot.PresentedViewController != null)
        //        {
        //            var presentedViewController = windowRoot.PresentedViewController;

        //            // the presented ViewController can also have a stack of ViewControllers
        //            if(presentedViewController.GetType().Equals(typeof(UINavigationController)) || presentedViewController.GetType().IsSubclassOf(typeof(UINavigationController)))
        //                return (presentedViewController as UINavigationController).TopViewController;
        //            else
        //                return windowRoot.PresentedViewController;
        //        }

        //        if(windowRoot.GetType().Equals(typeof(UINavigationController)) || windowRoot.GetType().IsSubclassOf(typeof(UINavigationController)))
        //            return (windowRoot as UINavigationController).TopViewController;

        //        return windowRoot;
        //    }
        //}

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

#warning Need to reinsert ClearTop type functionality here
            //if (request.ClearTop)
            //    ClearBackStack();

            Show(view, request);
        }

        public void Show(IMvxIosView view, MvxViewModelRequest request)
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

                MasterNavigationController = null;
                ModalNavigationController = null;
                SplitViewController = null;

                return;
            }

            // check if viewController is a SplitViewController
            if(viewController is IMvxSplitViewController)
            {
                SplitViewController = viewController as IMvxSplitViewController;
                SetWindowRootViewController(viewController);

                MasterNavigationController = null;
                ModalNavigationController = null;
                TabBarViewController = null;

                return;
            }

            // check if viewController is trying to initialize a navigation stack
            if(attribute.WrapInNavigationController)
            {
                viewController = new MvxNavigationController(viewController);
                MasterNavigationController = viewController as MvxNavigationController;
                SetWindowRootViewController(viewController);

                ModalNavigationController = null;
                TabBarViewController = null;
                SplitViewController = null;

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

        //public override bool PresentModalViewController(UIViewController viewController, bool animated)
        //{
        //    // if there is currently a modal ViewController, dismiss it (otherwise nothing happens when presenting)
        //    if(_window.RootViewController.PresentedViewController != null)
        //        _window.RootViewController.DismissViewController(animated, null);

        //    _window.RootViewController.PresentViewController(viewController, animated, null);

        //    return true;
        //}

        public void Close(IMvxViewModel toClose)
        {
            //Close(toClose, true);
        }

        //public void Close(IMvxViewModel toClose, bool animated)
        //{
        //    // check if toClose is a modal ViewController that is NOT wrapped in a navigation controller
        //    if(_window.RootViewController.PresentedViewController != null
        //       && _window.RootViewController.PresentedViewController as MvxNavigationController == null)
        //    {
        //        var mvxIosView = _window.RootViewController.PresentedViewController.GetIMvxIosView();
        //        if(mvxIosView != null)
        //        {
        //            if(mvxIosView.ViewModel == toClose)
        //            {
        //                _window.RootViewController.DismissViewController(animated, null);
        //                return;
        //            }
        //        }
        //    }

        //    if(CurrentTopViewController.NavigationController == null)
        //    {
        //        MvxTrace.Warning($"Don't know how to close ViewModel of type: {toClose.GetType().Name} - There is no current NavigationController");
        //        return;
        //    }

        //    // check if the current navigation controller is ModalNavigationController
        //    if(CurrentTopViewController.NavigationController == ModalNavigationController)
        //    {
        //        // if the ViewModel to close is the root of the modal navigation stack, then close the entire stack
        //        CloseModalViewController(toClose, animated);
        //        return;
        //    }

        //    if(CurrentTopViewController.NavigationController == MasterNavigationController)
        //    {
        //        CloseViewControllerInNavigationController(toClose, MasterNavigationController, animated);
        //        return;
        //    }
        //}

        //private void CloseViewControllerInNavigationController(IMvxViewModel toClose, UINavigationController navController, bool animated)
        //{
        //    // check if toClose is the top most ViewController
        //    var topViewController = navController.TopViewController.GetIMvxIosView();
        //    if(topViewController.ViewModel == toClose)
        //    {
        //        navController.PopViewController(animated);
        //        return;
        //    }

        //    // loop stack 
        //    foreach(var viewController in navController.ViewControllers)
        //    {
        //        var mvxView = viewController.GetIMvxIosView();
        //        if(mvxView.ViewModel == toClose)
        //        {
        //            var newViewControllers = navController.ViewControllers.Where(v => v != viewController).ToArray();
        //            navController.ViewControllers = newViewControllers;
        //            break;
        //        }
        //    }
        //}

        //public void CloseModalViewController(IMvxViewModel toClose, bool animated)
        //{
        //    if(_window.RootViewController.PresentedViewController == null)
        //        return;

        //    if(_window.RootViewController.PresentedViewController == ModalNavigationController)
        //    {
        //        var modalViewController = ModalNavigationController.ViewControllers.First();

        //        var mvxIosView = modalViewController.GetIMvxIosView();
        //        if(mvxIosView.ViewModel == toClose)
        //        {
        //            _window.RootViewController.DismissViewController(animated, null);
        //            ModalNavigationController = null;
        //        }
        //        else
        //        {
        //            CloseViewControllerInNavigationController(toClose, ModalNavigationController, animated);
        //        }

        //        return;
        //    }

        //    // if modal presented is not part of any stack, close it anyways
        //    _window.RootViewController.DismissViewController(animated, null);
        //}

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if(hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            _window.RootViewController.DismissViewController(false, null);
            ModalNavigationController = null;
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
                throw new MvxException("Please remember to set PresentationAttributes!");

            return attributes;
        }

        //private bool IsClassOrSubclass(Type source, Type target) => source.Equals(target) || source.IsSubclassOf(target);
    }
}

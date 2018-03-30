﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;
using UIKit;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Ios.Presenters
{
    public class MvxIosViewPresenter : MvxAttributeViewPresenter, IMvxIosViewPresenter
    {
        protected readonly IUIApplicationDelegate _applicationDelegate;
        protected readonly UIWindow _window;

        public UINavigationController MasterNavigationController { get; protected set; }

        public List<UIViewController> ModalViewControllers { get; protected set; } = new List<UIViewController>();

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public IMvxSplitViewController SplitViewController { get; protected set; }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (MasterNavigationController == null &&
               (TabBarViewController == null ||
               !TabBarViewController.CanShowChildView()))
            {
                MvxLog.Instance.Trace($"PresentationAttribute nor MasterNavigationController found for {viewType.Name}. " +
                    $"Assuming Root presentation");
                return new MvxRootPresentationAttribute() { WrapInNavigationController = true, ViewType = viewType, ViewModelType = viewModelType };
            }

            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. " +
                $"Assuming animated Child presentation");
            return new MvxChildPresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
        }

        public override MvxBasePresentationAttribute GetOverridePresentationAttribute(MvxViewModelRequest request, Type viewType)
        {
            if (viewType?.GetInterface(nameof(IMvxOverridePresentationAttribute)) != null)
            {
                var viewInstance = this.CreateViewControllerFor(viewType, null) as UIViewController;
                using (viewInstance)
                {
                    var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute(request);

                    if (presentationAttribute == null)
                    {
                        MvxLog.Instance.Warn("Override PresentationAttribute null. Falling back to existing attribute.");
                    }
                    else
                    {
                        if (presentationAttribute.ViewType == null)
                            presentationAttribute.ViewType = viewType;

                        if (presentationAttribute.ViewModelType == null)
                            presentationAttribute.ViewModelType = request.ViewModelType;

                        return presentationAttribute;
                    }
                }
            }

            return null;
        }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxRootPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowRootViewController(viewController, (MvxRootPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseRootViewController(viewModel, (MvxRootPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxChildPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowChildViewController(viewController, (MvxChildPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseChildViewController(viewModel, (MvxChildPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowTabViewController(viewController, (MvxTabPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseTabViewController(viewModel, (MvxTabPresentationAttribute)attribute)
                });


            AttributeTypesToActionsDictionary.Add(
                typeof(MvxModalPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowModalViewController(viewController, (MvxModalPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseModalViewController(viewModel, (MvxModalPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxMasterSplitViewPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowMasterSplitViewController(viewController, (MvxMasterSplitViewPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseMasterSplitViewController(viewModel, (MvxMasterSplitViewPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxDetailSplitViewPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowDetailSplitViewController(viewController, (MvxDetailSplitViewPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseDetailSplitViewController(viewModel, (MvxDetailSplitViewPresentationAttribute)attribute)
                });
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

                CloseModalViewControllers();
                CloseSplitViewController();

                return;
            }

            // check if viewController is a SplitViewController
            if (viewController is IMvxSplitViewController splitController)
            {
                SplitViewController = splitController;

                // set root
                SetupWindowRootNavigation(viewController, attribute);

                CloseModalViewControllers();
                CloseTabBarViewController();

                return;
            }

            // set root initiating stack navigation or just a plain controller
            SetupWindowRootNavigation(viewController, attribute);

            CloseModalViewControllers();
            CloseTabBarViewController();
            CloseSplitViewController();
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            if (attribute.WrapInNavigationController)
            {
                MasterNavigationController = CreateNavigationController(viewController);

                SetWindowRootViewController(MasterNavigationController, attribute);
            }
            else
            {
                SetWindowRootViewController(viewController, attribute);

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

        public override void Close(IMvxViewModel viewModel)
        {
            GetPresentationAttributeAction(new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute).CloseAction.Invoke(viewModel, attribute);
        }

        #region Close implementations

        protected virtual bool CloseRootViewController(IMvxViewModel viewModel, MvxRootPresentationAttribute attribute)
        {
            MvxLog.Instance.Warn($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return false;
        }

        protected virtual bool CloseChildViewController(IMvxViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            // if there are modals presented
            if (ModalViewControllers.Any())
            {
                foreach (var modalNav in ModalViewControllers.Where(v => v is UINavigationController))
                {
                    if (TryCloseViewControllerInsideStack((UINavigationController)modalNav, viewModel))
                        return true;
                }
            }

            //if the current root is a TabBarViewController, delegate close responsibility to it
            if (TabBarViewController != null && TabBarViewController.CloseChildViewModel(viewModel))
                return true;

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel))
                return true;

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel))
                return true;

            return false;
        }

        protected virtual bool CloseTabViewController(IMvxViewModel viewModel, MvxTabPresentationAttribute attribute)
        {
            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return true;

            return false;
        }

        protected virtual bool CloseModalViewController(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            return CloseModalViewController(viewModel);
        }

        protected virtual bool CloseMasterSplitViewController(IMvxViewModel viewModel, MvxMasterSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel))
                return true;

            return true;
        }

        protected virtual bool CloseDetailSplitViewController(IMvxViewModel viewModel, MvxDetailSplitViewPresentationAttribute attribute)
        {
            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel))
                return true;

            return true;
        }

        protected virtual bool CloseModalViewController(IMvxViewModel toClose)
        {
            if (ModalViewControllers == null || !ModalViewControllers.Any())
                return false;

            // check for plain modals
            var modalToClose = ModalViewControllers.FirstOrDefault(v => v is IMvxIosView && v.GetIMvxIosView().ViewModel == toClose);
            if (modalToClose != null)
            {
                CloseModalViewController(modalToClose);
                return true;
            }

            // check for modal navigation stacks
            UIViewController controllerToClose = null;
            foreach (var vc in ModalViewControllers.Where(v => v is UINavigationController))
            {
                var root = ((UINavigationController)vc).ViewControllers.FirstOrDefault();
                if (root != null && root.GetIMvxIosView().ViewModel == toClose)
                {
                    controllerToClose = vc;
                    break;
                }
            }
            if (controllerToClose != null)
            {
                CloseModalViewController(controllerToClose);
                return true;
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
            var controllers = navController.ViewControllers.ToList();
            var controllerToClose = controllers.FirstOrDefault(vc => vc.GetIMvxIosView().ViewModel == toClose);
            if (controllerToClose != null)
            {
                controllers.Remove(controllerToClose);
                navController.ViewControllers = controllers.ToArray();

                return true;
            }

            return false;
        }

        #endregion

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected virtual void PushViewControllerIntoStack(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            navigationController.PushViewController(viewController, animated);

            if (viewController is IMvxTabBarViewController tabBarController)
                TabBarViewController = tabBarController;
        }

        protected virtual void CloseMasterNavigationController()
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

        public virtual void CloseModalViewController(UIViewController viewController)
        {
            if (viewController == null)
                return;

            if (viewController is UINavigationController modalNavController)
            {
                foreach (var item in modalNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            viewController.DismissViewController(true, null);
            ModalViewControllers.Remove(viewController);
        }

        public virtual void CloseModalViewControllers()
        {
            while (ModalViewControllers.Any())
            {
                CloseModalViewController(ModalViewControllers.LastOrDefault());
            }
        }

        public virtual void CloseTabBarViewController()
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

        protected virtual void CloseSplitViewController()
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

        protected void RemoveWindowSubviews()
        {
            foreach (var v in _window.Subviews)
                v.RemoveFromSuperview();
        }

        public virtual void ShowModalViewController(UIViewController viewController, bool animated)
        {
            ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller, MvxRootPresentationAttribute attribute = null)
        {
            RemoveWindowSubviews();

            if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
            {
                _window.RootViewController = controller;
                return;
            }

            UIView.Transition(
                _window, attribute.AnimationDuration, attribute.AnimationOptions,
                () => _window.RootViewController = controller, null
            );
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Presenters
{
#nullable enable
    public class MvxIosViewPresenter : MvxAttributeViewPresenter, IMvxIosViewPresenter
    {
        private readonly MvxIosMajorVersionChecker _iosVersion13Checker = new MvxIosMajorVersionChecker(13);

        protected IUIApplicationDelegate ApplicationDelegate { get; }
        protected UIWindow Window { get; }

        public UINavigationController? MasterNavigationController { get; protected set; }

        public UIViewController? PopoverViewController { get; protected set; }

        public List<UIViewController> ModalViewControllers { get; } = new List<UIViewController>();

        public IMvxTabBarViewController? TabBarViewController { get; protected set; }

        public IMvxPageViewController? PageViewController { get; protected set; }

        public IMvxSplitViewController? SplitViewController { get; protected set; }

        public MvxIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            ApplicationDelegate = applicationDelegate;
            Window = window;
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            ValidateArguments(viewModelType, viewType);

            if (MasterNavigationController == null &&
                TabBarViewController?.CanShowChildView() != true)
            {
                MvxLogHost.GetLog<MvxIosViewPresenter>()?.LogTrace(
                    $"PresentationAttribute nor MasterNavigationController found for {viewType.Name}. Assuming Root presentation");
                return new MvxRootPresentationAttribute
                {
                    WrapInNavigationController = true,
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            MvxLogHost.GetLog<MvxIosViewPresenter>()?.LogTrace(
                $"PresentationAttribute not found for {viewType.Name}. Assuming animated Child presentation");

            return new MvxChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        public override object? CreateOverridePresentationAttributeViewInstance(Type viewType)
        {
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            return this.CreateViewControllerFor(viewType, null);
        }

        public override void RegisterAttributeTypes()
        {
            if (AttributeTypesToActionsDictionary == null)
                throw new InvalidOperationException("Cannot register attribute types on null dictionary");

            AttributeTypesToActionsDictionary.Register<MvxRootPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowRootViewController(viewController, attribute, request);
                },
                CloseRootViewController);

            AttributeTypesToActionsDictionary.Register<MvxChildPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowChildViewController(viewController, attribute, request);
                },
                CloseChildViewController);

            AttributeTypesToActionsDictionary.Register<MvxTabPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowTabViewController(viewController, attribute, request);
                },
                CloseTabViewController);

            AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowPageViewController(viewController, attribute, request);
                },
                ClosePageViewController);

            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowModalViewController(viewController, attribute, request);
                },
                CloseModalViewController);

            AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    var splitAttribute = attribute;
                    return splitAttribute.Position switch
                    {
                        MasterDetailPosition.Master =>
                            ShowMasterSplitViewController(viewController, splitAttribute, request),
                        MasterDetailPosition.Detail =>
                            ShowDetailSplitViewController(viewController, splitAttribute, request),
                        _ => Task.FromResult(true)
                    };
                },
                (viewModel, attribute) =>
                {
                    var splitAttribute = attribute;
                    return splitAttribute.Position switch
                    {
                        MasterDetailPosition.Master => CloseMasterSplitViewController(viewModel, splitAttribute),
                        MasterDetailPosition.Detail => CloseDetailSplitViewController(viewModel, splitAttribute),
                        _ => CloseDetailSplitViewController(viewModel, splitAttribute)
                    };
                });

            RegisterPopoverAttributeType();
        }

        protected virtual void RegisterPopoverAttributeType()
        {
            AttributeTypesToActionsDictionary.Register<MvxPopoverPresentationAttribute>(
                (viewType, attribute, request) =>
                {
                    var viewController = (UIViewController)this.CreateViewControllerFor(request);
                    return ShowPopoverViewController(viewController, attribute, request);
                },
                ClosePopoverViewController);
        }

        protected virtual Task<bool> ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            return viewController switch
            {
                // check if viewController is a TabBarController
                IMvxTabBarViewController tabBarController =>
                    ShowTabBarRootViewController(viewController, attribute, tabBarController),
                // check if viewController is a PageViewController
                IMvxPageViewController pageViewController =>
                    ShowPageRootViewController(viewController, attribute, pageViewController),
                // check if viewController is a SplitViewController
                IMvxSplitViewController splitController =>
                    ShowSplitRootViewController(viewController, attribute, splitController),
                // set root initiating stack navigation or just a plain controller
                _ => ShowRootViewController(viewController, attribute)
            };
        }

        private async Task<bool> ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(false)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(false)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(false)) return false;
            return true;
        }

        private async Task<bool> ShowSplitRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxSplitViewController splitController)
        {
            SplitViewController = splitController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(false)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(false)) return false;

            return true;
        }

        private async Task<bool> ShowPageRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxPageViewController pageViewController)
        {
            PageViewController = pageViewController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(false)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(false)) return false;

            return true;
        }

        private async Task<bool> ShowTabBarRootViewController(
            UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxTabBarViewController tabBarController)
        {
            TabBarViewController = tabBarController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;

            return true;
        }

        public override Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            return hint switch
            {
                null => throw new ArgumentNullException(nameof(hint)),
                MvxPagePresentationHint pagePresentationHint when ChangePagePresentation(pagePresentationHint) =>
                    Task.FromResult(true),
                _ => base.ChangePresentation(hint)
            };
        }

        private bool ChangePagePresentation(MvxPagePresentationHint pagePresentationHint)
        {
            if (!(TabBarViewController is UITabBarController tabsController) ||
                tabsController.ViewControllers == null)
            {
                return false;
            }

            foreach (var vc in tabsController.ViewControllers)
            {
                IMvxIosView? tabView;

                if (vc is UINavigationController navigationController)
                {
                    var root = navigationController.ViewControllers?.FirstOrDefault();
                    tabView = root.GetIMvxIosView();
                }
                else
                {
                    tabView = vc.GetIMvxIosView();
                }

                var viewModelType = tabView.GetViewModelType();
                if (viewModelType == null || viewModelType != pagePresentationHint.ViewModel) continue;

                tabsController.SelectedViewController = vc;
                return true;
            }

            return false;
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            ValidateArguments(viewController, attribute);

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

        protected virtual Task<bool> ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            if (viewController is IMvxSplitViewController)
                throw new MvxException("A SplitViewController cannot be presented as a child. Consider using Root instead");

            if (PopoverViewController != null)
            {
                return ShowPopoverViewControllerChild(viewController, attribute);
            }

            if (ModalViewControllers.Count > 0)
            {
                return ShowModalViewControllerChild(viewController, attribute);
            }

            if (TabBarViewController != null && TabBarViewController.ShowChildView(viewController))
            {
                return Task.FromResult(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return Task.FromResult(true);
            }

            throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        private Task<bool> ShowModalViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                PushViewControllerIntoStack(modalNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new MvxException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
        }

        private Task<bool> ShowPopoverViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (PopoverViewController is UINavigationController popoverNavController)
            {
                PushViewControllerIntoStack(popoverNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new MvxException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain popover view presented!");
        }

        protected virtual Task<bool> ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

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
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowPageViewController(
            UIViewController viewController,
            MvxPagePresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            if (PageViewController == null)
                throw new MvxException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest? request)
        {
            ValidateArguments(viewController, attribute);

            // Content size should be set to a target view controller, not the navigation one
            if (attribute.PreferredContentSize != default)
            {
                viewController.PreferredContentSize = attribute.PreferredContentSize;
            }

            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;
            if (_iosVersion13Checker.IsVersionOrHigher && viewController.PresentationController != null)
            {
                viewController.PresentationController.Delegate =
                    new MvxModalPresentationControllerDelegate(this, viewController, attribute);
            }

            // Check if there is a modal already presented first. Otherwise use the window root
            var modalHost = ModalViewControllers.LastOrDefault() ?? Window.RootViewController;
            if (modalHost != null)
            {
                modalHost.PresentViewController(
                    viewController,
                    attribute.Animated,
                    null);

                ModalViewControllers.Add(viewController);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        protected virtual async Task<bool> ShowPopoverViewController(
            UIViewController viewController,
            MvxPopoverPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            if (PopoverViewController != null)
                throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as popover, but there is already a popover present!");

            // Content size should be set to a target view controller, not the navigation one
            if (attribute.PreferredContentSize != default)
            {
                viewController.PreferredContentSize = attribute.PreferredContentSize;
            }

            // setup popover based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            // Check if there is a modal already presented first. Otherwise use the topmost view controller.
            var viewHost = ModalViewControllers.LastOrDefault() ?? Window.RootViewController;
            if (viewHost == null)
                throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as popover, but could not find a view host!");

            viewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;

            var presentationController = viewController.PopoverPresentationController;
            presentationController.PermittedArrowDirections = attribute.PermittedArrowDirections;

            var sourceProvider = Mvx.IoCProvider.Resolve<IMvxPopoverPresentationSourceProvider>();
            sourceProvider.SetSource(presentationController);
            presentationController.Delegate = new MvxPopoverPresentationControllerDelegate(this);

            PopoverViewController = viewController;
            await viewHost.PresentViewControllerAsync(viewController, attribute.Animated).ConfigureAwait(true);
            return true;
        }

        protected virtual Task<bool> ShowMasterSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            if (SplitViewController == null)
                throw new MvxException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowDetailSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            ValidateArguments(viewController, attribute);

            if (SplitViewController == null)
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseRootViewController(IMvxViewModel viewModel, MvxRootPresentationAttribute attribute)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            MvxLogHost.GetLog<MvxIosViewPresenter>()?.LogWarning(
                "Ignored attempt to close the window root (ViewModel type: {viewModelType}", viewModel.GetType().Name);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseChildViewController(IMvxViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            // if a popover is presented
            if (PopoverViewController is UINavigationController popoverNav &&
                TryCloseViewControllerInsideStack(popoverNav, viewModel, attribute))
            {
                return Task.FromResult(true);
            }

            // if there are modals presented
            if (ModalViewControllers.Count > 0 && CloseModalChildViewController(viewModel, attribute))
                return Task.FromResult(true);

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if (TabBarViewController?.CloseChildViewModel(viewModel) == true)
                return Task.FromResult(true);

            if (SplitViewController?.CloseChildViewModel(viewModel, attribute) == true)
                return Task.FromResult(true);

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        private bool CloseModalChildViewController(IMvxViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            foreach (var modalNav in ModalViewControllers.OfType<UINavigationController>())
            {
                if (TryCloseViewControllerInsideStack(modalNav, viewModel, attribute))
                    return true;
            }

            return false;
        }

        protected virtual Task<bool> CloseTabViewController(IMvxViewModel viewModel, MvxTabPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> ClosePageViewController(IMvxViewModel viewModel, MvxPagePresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseMasterSplitViewController(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseDetailSplitViewController(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseModalViewController(IMvxViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (ModalViewControllers.Count == 0)
                return Task.FromResult(false);

            // check for plain modals
            var modalToClose =
                ModalViewControllers.Find(v => v is IMvxIosView iosView && iosView.ViewModel == viewModel);
            if (modalToClose != null)
            {
                return CloseModalViewController(modalToClose, attribute);
            }

            // check for modal navigation stacks
            UIViewController? controllerToClose = null;
            foreach (var vc in ModalViewControllers.OfType<UINavigationController>())
            {
                var root = vc.ViewControllers?.FirstOrDefault();
                if (root != null && root.GetIMvxIosView()?.ViewModel == viewModel)
                {
                    controllerToClose = vc;
                    break;
                }
            }

            if (controllerToClose != null)
            {
                return CloseModalViewController(controllerToClose, attribute);
            }

            return Task.FromResult(false);
        }

        protected virtual Task<bool> ClosePopoverViewController(IMvxViewModel viewModel, MvxPopoverPresentationAttribute attribute)
        {
            ValidateArguments(viewModel, attribute);

            if (PopoverViewController == null)
                return Task.FromResult(false);

            // check for plain popover
            if (PopoverViewController is IMvxIosView iosView && iosView.ViewModel == viewModel)
            {
                return ClosePopoverViewController(PopoverViewController, attribute);
            }

            // check for popover navigation stack
            UIViewController? controllerToClose = null;
            if (PopoverViewController is UINavigationController vc)
            {
                var root = vc.ViewControllers?.FirstOrDefault();
                if (root is IMvxIosView rootIosView && rootIosView.ViewModel == viewModel)
                {
                    controllerToClose = vc;
                }
            }

            if (controllerToClose != null)
            {
                return ClosePopoverViewController(controllerToClose, attribute);
            }

            return Task.FromResult(false);
        }

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, IMvxViewModel toClose, MvxChildPresentationAttribute attribute)
        {
            ValidateArguments(navController, attribute);

            if (toClose == null)
                throw new ArgumentNullException(nameof(toClose));

            // check for top view controller
            var topView = navController.TopViewController;
            if (topView is IMvxIosView iosView && iosView.ViewModel == toClose)
            {
                navController.PopViewController(attribute.Animated);
                return true;
            }

            // loop through stack
            var controllers = navController.ViewControllers?.ToList();
            var controllerToClose = controllers?.Find(vc => vc is IMvxIosView iView && iView.ViewModel == toClose);
            if (controllerToClose != null)
            {
                controllers!.Remove(controllerToClose);
                navController.ViewControllers = controllers.ToArray();

                return true;
            }

            return false;
        }

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            if (viewController == null)
                throw new ArgumentNullException(nameof(viewController));

            return new MvxNavigationController(viewController);
        }

        protected virtual void PushViewControllerIntoStack(
            UINavigationController navigationController, UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            ValidateArguments(navigationController, attribute);

            navigationController.PushViewController(viewController, attribute.Animated);

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

        public virtual async Task<bool> CloseModalViewController(
            UIViewController viewController, MvxModalPresentationAttribute attribute)
        {
            ValidateArguments(viewController, attribute);

            if (viewController is UINavigationController modalNavController &&
                modalNavController.ViewControllers != null)
            {
                foreach (var item in modalNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated).ConfigureAwait(true);
            ModalViewControllers.Remove(viewController);
            return true;
        }

        public virtual async Task<bool> CloseModalViewControllers()
        {
            while (ModalViewControllers.Count > 0)
            {
                var didClose =
                    await CloseModalViewController(ModalViewControllers.Last(),
                        new MvxModalPresentationAttribute()).ConfigureAwait(true);

                if (!didClose)
                    return false;
            }

            return true;
        }

        public virtual async Task<bool> ClosePopoverViewController(UIViewController viewController, MvxPopoverPresentationAttribute attribute)
        {
            ValidateArguments(viewController, attribute);

            if (viewController is UINavigationController popoverNavController && popoverNavController.ViewControllers != null)
            {
                foreach (var item in popoverNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated).ConfigureAwait(true);
            PopoverViewController = null;
            return true;
        }

        public virtual Task<bool> CloseTabBarViewController()
        {
            if (TabBarViewController == null)
                return Task.FromResult(true);

            if (TabBarViewController is UITabBarController tabsController
                && tabsController.ViewControllers != null)
            {
                foreach (var item in tabsController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            TabBarViewController = null;
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseSplitViewController()
        {
            if (SplitViewController == null)
                return Task.FromResult(true);

            if (SplitViewController is UISplitViewController splitController)
            {
                foreach (var item in splitController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            SplitViewController = null;
            return Task.FromResult(true);
        }

        protected void RemoveWindowSubviews()
        {
            foreach (var v in Window.Subviews)
                v.RemoveFromSuperview();
        }

        public virtual Task<bool> ShowModalViewController(UIViewController viewController, bool animated)
        {
            return ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller, MvxRootPresentationAttribute? attribute = null)
        {
            RemoveWindowSubviews();

            if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
            {
                Window.RootViewController = controller;
                return;
            }

            UIView.Transition(
                Window, attribute.AnimationDuration, attribute.AnimationOptions,
                () => Window.RootViewController = controller, null
            );
        }

        // Called if popover was dismissed by tapping outside view.
        public virtual void ClosedPopoverViewController()
        {
            PopoverViewController = null;
        }

        // Called if the modal was dismissed by user (perhaps tapped background or form sheet swiped down)
        public virtual ConfiguredTaskAwaitable<bool> ClosedModalViewController(UIViewController viewController,
            MvxModalPresentationAttribute attribute)
        {
            return CloseModalViewController(viewController, attribute).ConfigureAwait(false);
        }

        private static void ValidateArguments(Type viewModelType, Type viewType)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));
        }

        private static void ValidateArguments(UIViewController viewController, MvxBasePresentationAttribute attribute)
        {
            if (viewController == null)
                throw new ArgumentNullException(nameof(viewController));

            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

        private static void ValidateArguments(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }
    }
#nullable restore
}

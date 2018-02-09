// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platform.Tvos.Views.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Platform.Tvos.Views.Presenters
{
    public class MvxTvosViewPresenter
        : MvxAttributeViewPresenter, IMvxTvosViewPresenter
    {
        private readonly IUIApplicationDelegate _applicationDelegate;
        protected IUIApplicationDelegate ApplicationDelegate => _applicationDelegate;

        private readonly UIWindow _window;
        protected UIWindow Window => _window;

        public UINavigationController MasterNavigationController { get; protected set; }

        public List<UIViewController> ModalViewControllers { get; protected set; } = new List<UIViewController>();

        public IMvxTabBarViewController TabBarViewController { get; protected set; }

        public MvxSplitViewController SplitViewController { get; protected set; }

        public MvxTvosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (MasterNavigationController == null &&
               TabBarViewController == null ||
               !TabBarViewController.CanShowChildView())
            {
                MvxLog.Instance.Trace($"PresentationAttribute nor MasterNavigationController found for {viewType.Name}. " +
                    $"Assuming Root presentation");
                return new MvxRootPresentationAttribute()
                {
                    WrapInNavigationController = true,
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }
            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. " +
                $"Assuming animated Child presentation");
            return new MvxChildPresentationAttribute()
            {
                ViewType = viewType,
                ViewModelType = viewModelType
            };

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

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxRootPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowRootViewController(viewController,
                                               (MvxRootPresentationAttribute)attribute,
                                               request);
                    },
                    CloseAction = (viewModel, attribute) => CloseRootViewController(viewModel,
                                                                                    (MvxRootPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
              typeof(MvxChildPresentationAttribute),
              new MvxPresentationAttributeAction
              {
                  ShowAction = (viewType, attribute, request) =>
                  {
                      var viewController = (UIViewController)this.CreateViewControllerFor(request);
                      ShowChildViewController(viewController,
                                              (MvxChildPresentationAttribute)attribute,
                                              request);
                  },
                  CloseAction = (viewModel, attribute) => CloseChildViewController(viewModel,
                                                                                   (MvxChildPresentationAttribute)attribute)
              });


            AttributeTypesToActionsDictionary.Add(
                typeof(MvxTabPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowTabViewController(viewController,
                                              (MvxTabPresentationAttribute)attribute,
                                              request);
                    },
                    CloseAction = (viewModel, attribute) => CloseTabViewController(viewModel,
                                                                                   (MvxTabPresentationAttribute)attribute)
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
                typeof(MvxMasterDetailPresentationAttribute),
              new MvxPresentationAttributeAction
              {
                  ShowAction = (viewType, attribute, request) =>
                  {
                      var viewController = (UIViewController)this.CreateViewControllerFor(request);
                      ShowMasterDetailSplitViewController(viewController, (MvxMasterDetailPresentationAttribute)attribute, request);
                  },
                  CloseAction = (viewModel, attribute) => CloseMasterSplitViewController(viewModel, (MvxMasterDetailPresentationAttribute)attribute)
              });
        }

        protected virtual bool CloseRootViewController(IMvxViewModel viewModel,
                                     MvxRootPresentationAttribute attribute)
        {
            MvxLog.Instance.Warn($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");
            return false;
        }

        protected virtual bool CloseChildViewController(IMvxViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.Any())
            {
                foreach (var navController in ModalViewControllers.Where(v => v is UINavigationController))
                {
                    if (TryCloseViewControllerInsideStack((UINavigationController)navController, viewModel))
                        return true;
                }
            }

            if (TabBarViewController != null && TabBarViewController.CloseChildViewModel(viewModel))
                return true;

            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel))
                return true;

            return false;
        }

        public void CloseTabBarViewController()
        {
            if (TabBarViewController == null)
                return;

            if (TabBarViewController is UITabBarController tabController
               && tabController.ViewControllers != null)
            {
                foreach (var viewController in tabController.ViewControllers)
                    viewController.DidMoveToParentViewController(null);
            }

            TabBarViewController = null;
        }

        protected virtual bool CloseTabViewController(IMvxViewModel viewModel,
                                    MvxTabPresentationAttribute attribute)
        {
            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return true;

            return false;
        }

        protected virtual bool CloseModalViewController(IMvxViewModel viewModel,
                                                        MvxModalPresentationAttribute attribute)
        {
            return CloseModalViewController(viewModel);
        }

        protected virtual bool CloseModalViewController(IMvxViewModel viewModel)
        {
            if (ModalViewControllers == null || !ModalViewControllers.Any())
                return false;

            var modal = ModalViewControllers
                .FirstOrDefault(v => v is IMvxTvosView && v.GetIMvxTvosView().ViewModel == viewModel);
            if (modal != null)
            {
                CloseModalViewController(modal);
                return true;
            }

            UIViewController viewController = null;
            foreach (var vc in ModalViewControllers.Where(v => v is UINavigationController))
            {
                var rootViewController = ((UINavigationController)vc).ViewControllers.FirstOrDefault();
                if (rootViewController != null && rootViewController.GetIMvxTvosView().ViewModel == viewModel)
                {
                    viewController = vc;
                    break;
                }
            }
            if (viewController != null)
            {
                CloseModalViewController(viewController);
                return true;
            }

            return false;
        }

        protected virtual void CloseModalViewController(UIViewController viewController)
        {
            if (viewController == null)
                return;

            if (viewController is UINavigationController navController)
            {
                foreach (var view in navController.ViewControllers)
                    view.DidMoveToParentViewController(null);
            }

            viewController.DismissViewController(true, null);
            ModalViewControllers.Remove(viewController);
        }

        public virtual void CloseModalViewController()
        {
            MasterNavigationController.PopViewController(true);
        }

        protected void CloseMasterNavigationController()
        {
            if (MasterNavigationController == null)
                return;
            if (MasterNavigationController.ViewControllers != null)
            {
                foreach (var v in MasterNavigationController.ViewControllers)
                    v.DidMoveToParentViewController(null);
            }
            MasterNavigationController = null;
        }

        protected virtual bool CloseMasterSplitViewController(IMvxViewModel viewModel, MvxMasterDetailPresentationAttribute attribute)
        {
            if (SplitViewController != null &&
                attribute.Position != MasterDetailPosition.Root &&
                CloseChildViewModel(viewModel))
                return true;
            else if (attribute.Position == MasterDetailPosition.Root)
                return false;
            return true;
        }

        protected virtual bool CloseDetailSplitViewController(IMvxViewModel viewModel, MvxMasterDetailPresentationAttribute attribute)
        {
            if (SplitViewController != null && CloseChildViewModel(viewModel))
                return true;
            return true;
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (!SplitViewController.ViewControllers.Any())
                return false;

            var toClose = SplitViewController.ViewControllers.ToList()
                                         .Select(v => v.GetIMvxTvosView())
                                         .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
            if (toClose != null)
            {
                var newStack = SplitViewController.ViewControllers.Where(v => v.GetIMvxTvosView() != toClose);
                SplitViewController.ViewControllers = newStack.ToArray();

                return true;
            }

            return false;
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

        public override void Close(IMvxViewModel viewModel)
        {
            GetPresentationAttributeAction(new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute)
                .CloseAction.Invoke(viewModel, attribute);
        }

        public override void Show(MvxViewModelRequest request)
        {
            GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute)
                .ShowAction.Invoke(attribute.ViewType, attribute, request);
        }

        protected virtual void ShowRootViewController(UIViewController viewController,
                                           MvxRootPresentationAttribute attribute,
                                           MvxViewModelRequest request)
        {
            if (viewController is IMvxTabBarViewController)
            {
                //NOTE clean up must be done first incase we are enbedding into a navigation controller
                //before setting the tab view controller, otherwise this will reset the view stack and your tab
                //controller will be null. 
                SetupWindowRootNavigation(viewController, attribute);
                this.TabBarViewController = (IMvxTabBarViewController)viewController;

                CloseModalViewControllers();

                return;
            }

            SetupWindowRootNavigation(viewController, attribute);

            CloseModalViewControllers();
            CloseTabBarViewController();
            CloseSplitViewController();
        }

        protected virtual void ShowChildViewController(UIViewController viewController,
                                                        MvxChildPresentationAttribute attribute,
                                                        MvxViewModelRequest request)
        {
            if (viewController is MvxSplitViewController)
                throw new MvxException("A SplitViewController can't be present in a child.  Consider using a Root instead.");

            if (ModalViewControllers.Any())
            {
                if (ModalViewControllers.LastOrDefault() is UINavigationController navigationController)
                {
                    PushViewControllerIntoStack(navigationController, viewController, attribute.Animated);
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

        protected virtual void ShowModalViewController(UIViewController viewController,
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

        protected virtual void ShowTabViewController(UIViewController viewController,
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

        protected virtual void ShowMasterDetailSplitViewController(
          UIViewController viewController,
            MvxMasterDetailPresentationAttribute attribute,
          MvxViewModelRequest request)
        {
            if (SplitViewController != null && attribute.Position == MasterDetailPosition.Master)
            {
                ShowMasterView(viewController, attribute.WrapInNavigationController);
            }
            else if (SplitViewController != null && attribute.Position == MasterDetailPosition.Detail)
            {
                ShowDetailView(viewController, attribute.WrapInNavigationController);
            }
            else if (viewController is MvxSplitViewController && attribute.Position == MasterDetailPosition.Root)
            {
                SplitViewController = (MvxSplitViewController)viewController;

                // set root
                SetupSplitViewWindowRootNavigation(viewController, attribute);

                CloseModalViewControllers();
                CloseTabBarViewController();
                return;
            }
            else
            {
                throw new MvxException("Trying to show a master page without a SplitViewController, this is not possible!");
            }
        }

        public virtual void ShowDetailView(UIViewController viewController, bool wrapInNavigationController)
        {
            viewController = wrapInNavigationController ?
                new MvxNavigationController(viewController) : viewController;

            SplitViewController.ShowDetailViewController(viewController, SplitViewController);
        }

        public virtual void ShowMasterView(UIViewController viewController, bool wrapInNavigationController)
        {
            var stack = SplitViewController.ViewControllers.ToList();

            viewController = wrapInNavigationController
                ? new MvxNavigationController(viewController) : viewController;

            if (stack.Any())
                stack.RemoveAt(0);

            stack.Insert(0, viewController);

            SplitViewController.ViewControllers = stack.ToArray();
        }
        public bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
            return true;
        }

        public virtual void CloseTopModalViewController()
        {
            CloseModalViewController(ModalViewControllers?.Last());
        }

        protected virtual void PushViewControllerIntoStack(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            navigationController.PushViewController(viewController, animated);

            if (viewController is IMvxTabBarViewController tabBarController)
                TabBarViewController = tabBarController;
        }

        protected void SetupWindowRootNavigation(UIViewController viewController,
                                                 MvxRootPresentationAttribute attribute)
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

        protected void SetupSplitViewWindowRootNavigation(UIViewController viewController,
                                               MvxMasterDetailPresentationAttribute attribute)
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

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            foreach (var v in _window.Subviews)
                v.RemoveFromSuperview();

            _window.AddSubview(controller.View);
            _window.RootViewController = controller;
        }

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navigationController,
                                                                IMvxViewModel viewModel)
        {
            var topViewController = navigationController.TopViewController.GetIMvxTvosView();
            if (topViewController != null && topViewController.ViewModel == viewModel)
            {
                navigationController.PopViewController(true);
                return true;
            }

            var viewControllers = navigationController.ViewControllers.ToList();
            var viewController = viewControllers.FirstOrDefault(vc => vc.GetIMvxTvosView().ViewModel == viewModel);
            if (viewController != null)
            {
                viewControllers.Remove(viewController);
                navigationController.ViewControllers = viewControllers.ToArray();

                return true;
            }

            return false;
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected void CloseModalViewControllers()
        {
            while (ModalViewControllers.Any())
            {
                CloseModalViewController(ModalViewControllers.LastOrDefault());
            }
        }

    }
}

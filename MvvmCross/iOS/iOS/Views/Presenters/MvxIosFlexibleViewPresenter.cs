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
    public class MvxIosFlexibleViewPresenter : MvxBaseIosViewPresenter
    {
        private UIViewController currentRootViewController;

        protected readonly IUIApplicationDelegate applicationDelegate;
        protected readonly UIWindow window;
        protected Lazy<Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>> attributeTypesToShowMethodDictionaryLazy;

        public MvxIosFlexibleViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
        {
            this.applicationDelegate = applicationDelegate;
            this.window = window;
            attributeTypesToShowMethodDictionaryLazy = new Lazy<Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>>(() => RegisterAttributeTypes());
        }

        protected virtual Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>> RegisterAttributeTypes()
        {
            var attributeTypesToShowMethodDictionary = new Dictionary<Type, Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest>>
            {
                {
                    typeof(MvxRootPresentationAttribute),
                    (vc, attribute, request) => ShowRootViewController(vc, (MvxRootPresentationAttribute)attribute, request)
                },

                {
                    typeof(MvxChildPresentationAttribute),
                    (vc, attribute, request) => ShowChildViewController(vc, (MvxChildPresentationAttribute)attribute, request)
                },

                {
                    typeof(MvxTabPresentationAttribute),
                    (vc, attribute, request) => ShowTabViewController(vc, (MvxTabPresentationAttribute)attribute, request)
                },

                {
                    typeof(MvxModalPresentationAttribute),
                    (vc, attribute, request) => ShowModalViewController(vc, (MvxModalPresentationAttribute)attribute, request)
                },

                {
                    typeof(MvxMasterSplitViewPresentationAttribute),
                    (vc, attribute, request) => ShowMasterSplitViewController(vc, (MvxMasterSplitViewPresentationAttribute)attribute, request)
                },

                {
                    typeof(MvxDetailSplitViewPresentationAttribute),
                    (vc, attribute, request) => ShowDetailSplitViewController(vc, (MvxDetailSplitViewPresentationAttribute)attribute, request)
                }
            };

            return attributeTypesToShowMethodDictionary;
        }

        protected virtual UINavigationController CreateNavigationViewController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected MvxBasePresentationAttribute GetPresentationAttributes(UIViewController viewController)
        {
            if (viewController is IMvxOverridePresentationAttribute v)
            {
                var presentationAttribute = v.PresentationAttribute();

                if (presentationAttribute != null)
                    return presentationAttribute;
            }

            // first try
            if (viewController.GetType().GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).FirstOrDefault() is MvxBasePresentationAttribute attribute)
            {
                return attribute;
            }

            UIViewController vc = null;
            var result = ExpandViewController(
                viewController,
                navVc =>
            {
                vc = navVc.TopViewController;
                return true;
            },
                tabBarVc =>
            {
                vc = (tabBarVc as UITabBarController)?.SelectedViewController;
                return true;
            },
                splitVc =>
            {
                vc = (splitVc as UISplitViewController)?.ViewControllers?.FirstOrDefault();
                return true;
            });

            // last try
            if (vc != null)
            {
                attribute = vc.GetType().GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).FirstOrDefault() as MvxBasePresentationAttribute;
                if (attribute != null)
                {
                    return attribute;
                }
            }

            if (currentRootViewController is IMvxTabBarViewController tabBarView && tabBarView.CanShowChildView(viewController))
            {
                MvxTrace.Trace($"PresentationAttribute not MasterNavigationController found for {viewController.GetType().Name}. Assuming Root presentation");
                return new MvxRootPresentationAttribute() { WrapInNavigationController = true };
            }

            MvxTrace.Trace($"PresentationAttribute not found for {viewController.GetType().Name}. Assuming animated Child presentation");
            return new MvxChildPresentationAttribute();
        }

        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            foreach (var v in window.Subviews)
                v.RemoveFromSuperview();

            window.AddSubview(controller.View);
            window.RootViewController = controller;
        }

        protected virtual bool ExpandCurrentViewController(
            Func<UINavigationController, bool> navAction,
            Func<IMvxTabBarViewController, bool> tabBarAction,
            Func<IMvxSplitViewController, bool> splitAction)
        {
            return ExpandViewController(currentRootViewController, navAction, tabBarAction, splitAction);
        }

        protected bool ExpandViewController(
            UIViewController vc,
            Func<UINavigationController, bool> navAction,
            Func<IMvxTabBarViewController, bool> tabBarAction,
            Func<IMvxSplitViewController, bool> splitAction)
        {
            if (vc is UINavigationController navVc)
            {
                return navAction?.Invoke(navVc) ?? false;
            }
            else if (vc is IMvxTabBarViewController tabBarVc)
            {
                return tabBarAction?.Invoke(tabBarVc) ?? false;
            }
            else if (vc is IMvxSplitViewController splitVc)
            {
                return splitAction?.Invoke(splitVc) ?? false;
            }

            return false;
        }

        protected bool SetCurrentRootViewController(UIViewController vc)
        {
            if (vc == null)
            {
                return false;
            }

            currentRootViewController = vc;
            return true;
        }

        #region Show

        public sealed override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);
            this.Show(view, request);
        }

        protected virtual void Show(IMvxIosView view, MvxViewModelRequest request)
        {
            var viewController = view as UIViewController;
            var attribute = GetPresentationAttributes(viewController);
            if (!attributeTypesToShowMethodDictionaryLazy.Value.TryGetValue(attribute.GetType(), out Action<UIViewController, MvxBasePresentationAttribute, MvxViewModelRequest> showAction))
                throw new KeyNotFoundException($"The type {attribute.GetType().Name} is not configured in the presenter dictionary");

            showAction.Invoke(viewController, attribute, request);
        }

        /// <summary>
        /// Shows the root view controller.
        /// Support all view controllers.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowRootViewController(
            UIViewController viewController,
            MvxRootPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            viewController = attribute.WrapInNavigationController ? CreateNavigationViewController(viewController) : viewController;
            SetCurrentRootViewController(viewController);
            SetWindowRootViewController(viewController);
        }

        /// <summary>
        /// Shows the child view controller.
        /// Support only UINavigationController and UITabBarController.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowChildViewController(
            UIViewController viewController,
            MvxChildPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var result = ExpandCurrentViewController(
                navVc =>
            {
                navVc.PushViewController(viewController, attribute.Animated);
                return true;
            },
                tabBarVc =>
            {
                return tabBarVc.ShowChildView(viewController);
            },
                splitVc =>
            {
                return false;
            });

            if (!result)
            {
                throw new MvxException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current Root!");
            }
        }

        /// <summary>
        /// Shows the tab view controller.
        /// Support only UITabBarController.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            string tabName = attribute.TabName;
            string tabIconName = attribute.TabIconName;
            var currentTabBarController = (currentRootViewController is UITabBarController tabBarView) // check UITabBarController
                ? (UIViewController)tabBarView
                : (currentRootViewController is UINavigationController navView) // check tabBar into UINavigationViewController
                         ? navView.TopViewController
                         : currentRootViewController;

            var result = ExpandViewController(
                currentTabBarController,
                navVc =>
            {
                return false;
            },
                tabBarVc =>
            {
                if (viewController is IMvxTabBarItemViewController tabBarItem)
                {
                    tabName = tabBarItem.TabName;
                    tabIconName = tabBarItem.TabIconName;
                }

                if (attribute.WrapInNavigationController)
                    viewController = CreateNavigationViewController(viewController);

                tabBarVc.ShowTabView(
                    viewController,
                    tabName,
                    tabIconName,
                    attribute.TabAccessibilityIdentifier);

                return true;
            },
                splitVc =>
            {
                return false;
            });

            if (!result)
            {
                throw new MvxException("Trying to show a tab without a TabBarViewController, this is not possible!");
            }
        }

        /// <summary>
        /// Shows the modal view controller.
        /// Support all view controllers.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationViewController(viewController);
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;
            if (attribute.PreferredContentSize != default(CGSize))
                viewController.PreferredContentSize = attribute.PreferredContentSize;

            currentRootViewController?.PresentViewController(viewController, attribute.Animated, null);
            SetCurrentRootViewController(viewController);
        }

        /// <summary>
        /// Shows the master split view controller.
        /// Support only UISplitViewController.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowMasterSplitViewController(
            UIViewController viewController,
            MvxMasterSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var result = ExpandCurrentViewController(
                navVc =>
            {
                return false;
            },
                tabBarVc =>
            {
                return false;
            },
                splitVc =>
            {
                splitVc.ShowMasterView(viewController, attribute.WrapInNavigationController);
                return true;
            });

            if (!result)
            {
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");
            }
        }

        /// <summary>
        /// Shows the detail split view controller.
        /// Support only UISplitViewController.
        /// </summary>
        /// <param name="viewController">View controller.</param>
        /// <param name="attribute">Attribute.</param>
        /// <param name="request">Request.</param>
        protected virtual void ShowDetailSplitViewController(
            UIViewController viewController,
            MvxDetailSplitViewPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            var result = ExpandCurrentViewController(
                navVc =>
             {
                 return false;
             },
                tabBarVc =>
             {
                 return false;
             },
                splitVc =>
             {
                 splitVc.ShowDetailView(viewController, attribute.WrapInNavigationController);
                 return true;
             });

            if (!result)
            {
                throw new MvxException("Trying to show a detail page without a SplitViewController, this is not possible!");
            }
        }

        #endregion

        #region Close

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            base.NativeModalViewControllerDisappearedOnItsOwn();
            if (TryCloseModalViewController(null))
            {
                return;
            }

            var result = ExpandCurrentViewController(
                navVc =>
            {
                navVc.PopToRootViewController(true);
                return true;
            },
                tabBarVc =>
            {
                return false;
            },
                splitVc =>
            {
                return false;
            });

            if (!result)
            {
                MvxTrace.Trace("Unrecognized on its own");
            }
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
            }
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (TryCloseModalViewController(toClose))
            {
                return;
            }

            var result = ExpandCurrentViewController(
                navVc =>
            {
                return CloseViewControllerInsideStack(navVc, toClose);
            },
                tabBarVc =>
            {
                return tabBarVc.CloseChildViewModel(toClose);
            },
                splitVc =>
            {
                return splitVc.CloseChildViewModel(toClose);
            });

            if (!result)
            {
                throw new MvxException($"Could not close ViewModel type {toClose.GetType().Name}");
            }
        }

        protected virtual bool TryCloseModalViewController(IMvxViewModel toClose)
        {
            // try get MvxModalPresentationAttribute or presentingViewController
            if (!(GetPresentationAttributes(currentRootViewController) is MvxModalPresentationAttribute) || currentRootViewController?.PresentingViewController == null)
            {
                return false;
            }

            var presentingVc = currentRootViewController.PresentingViewController;
            currentRootViewController.DismissViewController(true, null);

            return SetCurrentRootViewController(presentingVc);
        }

        protected virtual bool CloseViewControllerInsideStack(UINavigationController navController, IMvxViewModel toClose)
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

        #endregion
    }
}

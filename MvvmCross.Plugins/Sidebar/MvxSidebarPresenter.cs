// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Plugin.Sidebar.Extensions;
using MvvmCross.Plugin.Sidebar.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using UIKit;

namespace MvvmCross.Plugin.Sidebar
{
    public class MvxSidebarPresenter : MvxIosViewPresenter
    {
        protected virtual MvxSidebarViewController SideBarViewController { get; set; }

        public MvxSidebarPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
                    : base(applicationDelegate, window)
        {
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxSidebarPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = async (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        return await ShowSidebarViewController(viewController, (MvxSidebarPresentationAttribute)attribute, request);
                    },
                    CloseAction = async (viewModel, attribute) => CloseSidebarViewController(viewModel, (MvxSidebarPresentationAttribute)attribute)
                });
        }

        protected virtual async Task<bool> ShowSidebarViewController(
            UIViewController viewController,
            MvxSidebarPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SideBarViewController == null)
            {
                if (!await ShowRootViewController(new MvxSidebarViewController(), null, request)) return false;
            }

            switch (attribute.HintType)
            {
                case MvxPanelHintType.PopToRoot:
                    if (!ShowPanelAndPopToRoot(attribute, viewController)) return false;
                    break;

                case MvxPanelHintType.ResetRoot:
                    if (!ShowPanelAndResetToRoot(attribute, viewController)) return false;
                    break;

                case MvxPanelHintType.PushPanel:
                default:
                    if (!ShowPanel(attribute, viewController)) return false;
                    break;
            }

            if (!attribute.ShowPanel)
            {
                var menu = Mvx.IoCProvider.Resolve<IMvxSidebarViewController>();
                menu?.CloseMenu();
            }

            return true;
        }

        protected virtual bool ShowPanelAndPopToRoot(MvxSidebarPresentationAttribute attribute, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if (navigationController == null)
                return false;

            navigationController.PopToRootViewController(false);
            navigationController.PushViewController(viewController, false);

            return true;
        }

        protected virtual bool ShowPanelAndResetToRoot(MvxSidebarPresentationAttribute attribute, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if (navigationController == null)
                return false;

            navigationController.ViewControllers = new[] { viewController };

            switch (attribute.Panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    viewController.ShowMenuButton(SideBarViewController);
                    break;
                case MvxPanelEnum.CenterWithLeft:
                    viewController.ShowMenuButton(SideBarViewController, showLeft: true, showRight: false);
                    break;
                case MvxPanelEnum.CenterWithRight:
                    viewController.ShowMenuButton(SideBarViewController, showLeft: false, showRight: true);
                    break;
            }

            return true;
        }

        protected virtual bool ShowPanel(MvxSidebarPresentationAttribute attribute, UIViewController viewController)
        {
            var navigationController = SideBarViewController.NavigationController;

            if (navigationController == null)
                return false;

            switch (attribute.Panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    navigationController.PushViewController(viewController, attribute.Animated);
                    break;
                case MvxPanelEnum.CenterWithLeft:
                    navigationController.PushViewController(viewController, attribute.Animated);
                    viewController.ShowMenuButton(SideBarViewController, showLeft: true, showRight: false);
                    break;
                case MvxPanelEnum.CenterWithRight:
                    navigationController.PushViewController(viewController, attribute.Animated);
                    viewController.ShowMenuButton(SideBarViewController, showLeft: false, showRight: true);
                    break;
            }

            return true;
        }

        protected override async Task<bool> ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute, MvxViewModelRequest request)
        {
            // check if viewController is a MvxSidebarPanelController
            if (viewController is MvxSidebarViewController sidebarView)
            {
                MasterNavigationController = new MvxNavigationController();

                SideBarViewController = sidebarView;
                SideBarViewController.SetNavigationController(MasterNavigationController);

                SetWindowRootViewController(viewController, attribute);

                Mvx.IoCProvider.RegisterSingleton<IMvxSidebarViewController>(SideBarViewController);

                if (!await CloseModalViewControllers()) return false;
                if (!await CloseTabBarViewController()) return false;
                if (!await CloseSplitViewController()) return false;
                CloseMasterNavigationController();

                return true;
            }
            else
            {
                SideBarViewController = null;
                MasterNavigationController = null;
            
                return await base.ShowRootViewController(viewController, attribute, request);
            }
        }

        protected virtual bool CloseSidebarViewController(IMvxViewModel viewModel, MvxSidebarPresentationAttribute attribute)
        {
            if (SideBarViewController != null && SideBarViewController.CloseChildViewModel(viewModel))
                return true;

            return false;
        }
    }
}

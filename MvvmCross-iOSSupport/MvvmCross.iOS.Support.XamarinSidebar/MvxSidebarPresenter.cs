using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Support.XamarinSidebar.Extensions;
using MvvmCross.iOS.Support.XamarinSidebar.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Platform;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPresenter : MvxIosViewPresenter
    {
        protected virtual IMvxSidebarViewController SideBarViewController { get; set; }

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
                    ShowAction = (viewType, attribute, request) =>
                    {
                        var viewController = (UIViewController)this.CreateViewControllerFor(request);
                        ShowSidebarViewController(viewController, (MvxSidebarPresentationAttribute)attribute, request);
                    },
                    CloseAction = (viewModel, attribute) => CloseSidebarViewController(viewModel, (MvxSidebarPresentationAttribute)attribute)
                });
        }

        protected virtual void ShowSidebarViewController(
            UIViewController viewController,
            MvxSidebarPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (SideBarViewController == null)
                ShowRootViewController(new MvxSidebarViewController(), null, request);

            switch (attribute.HintType)
            {
                case MvxPanelHintType.PopToRoot:
                    ShowPanelAndPopToRoot(attribute.Panel, viewController);
                    break;

                case MvxPanelHintType.ResetRoot:
                    ShowPanelAndResetToRoot(attribute.Panel, viewController);
                    break;

                case MvxPanelHintType.PushPanel:
                default:
                    ShowPanel(attribute.Panel, viewController);
                    break;
            }

            if (!attribute.ShowPanel)
            {
                var menu = Mvx.Resolve<IMvxSidebarViewController>();
                menu?.CloseMenu();
            }
        }

        protected virtual bool ShowPanelAndPopToRoot(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if (navigationController == null)
                return false;

            navigationController.PopToRootViewController(false);
            navigationController.PushViewController(viewController, false);

            return true;
        }

        protected virtual bool ShowPanelAndResetToRoot(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if (navigationController == null)
                return false;

            navigationController.ViewControllers = new[] { viewController };

            switch (panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    viewController.ShowMenuButton(SideBarViewController as MvxSidebarViewController);
                    break;
                case MvxPanelEnum.CenterWithLeft:
                    viewController.ShowMenuButton(SideBarViewController as MvxSidebarViewController, showLeft: true, showRight: false);
                    break;
                case MvxPanelEnum.CenterWithRight:
                    viewController.ShowMenuButton(SideBarViewController as MvxSidebarViewController, showLeft: false, showRight: true);
                    break;
            }

            return true;
        }

        protected virtual bool ShowPanel(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if (navigationController == null)
                return false;

            switch (panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    navigationController.PushViewController(viewController, true);
                    break;
                case MvxPanelEnum.CenterWithLeft:
                    navigationController.PushViewController(viewController, true);
                    viewController.ShowMenuButton(SideBarViewController as MvxSidebarViewController, showLeft: true, showRight: false);
                    break;
                case MvxPanelEnum.CenterWithRight:
                    navigationController.PushViewController(viewController, true);
                    viewController.ShowMenuButton(SideBarViewController as MvxSidebarViewController, showLeft: false, showRight: true);
                    break;
            }

            return true;
        }

        protected override void ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute, MvxViewModelRequest request)
        {
            // check if viewController is a MvxSidebarPanelController
            if (viewController is IMvxSidebarViewController)
            {
                MasterNavigationController = new MvxNavigationController();

                SideBarViewController = viewController as IMvxSidebarViewController;
                SideBarViewController.SetNavigationController(MasterNavigationController);

                SetWindowRootViewController(viewController);

                Mvx.RegisterSingleton<IMvxSidebarViewController>(SideBarViewController);

                CleanupModalViewControllers();
                CloseTabBarViewController();
                CloseSplitViewController();
                CloseMasterNavigationController();

                return;
            }
            else
            {
                SideBarViewController = null;
                MasterNavigationController = null;
            }

            base.ShowRootViewController(viewController, attribute, request);
        }

        protected virtual bool CloseSidebarViewController(IMvxViewModel viewModel, MvxSidebarPresentationAttribute attribute)
        {
            if (SideBarViewController != null && SideBarViewController.CloseChildViewModel(viewModel))
                return true;

            return false;
        }
    }
}

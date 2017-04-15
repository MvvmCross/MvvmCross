using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
using MvvmCross.iOS.Support.XamarinSidebar.Extensions;
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

        protected override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();

            _attributeTypesToShowMethodDictionary.Add(
                typeof(MvxSidebarPresentationAttribute),
                (vc, attribute, request) => ShowSidebarViewController(vc, attribute as MvxSidebarPresentationAttribute, request));
        }

        protected virtual void ShowSidebarViewController(
            UIViewController viewController,
            MvxSidebarPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if(SideBarViewController == null)
                ShowRootViewController(new MvxSidebarViewController(), null, request);

            switch(attribute.HintType)
            {
                case MvxPanelHintType.PopToRoot:
                    ShowPanelAndPopToRoot(attribute.Panel, viewController);
                    break;

                case MvxPanelHintType.ResetRoot:
                    ShowPanelAndResetToRoot(attribute.Panel, viewController);
                    break;

                case MvxPanelHintType.ActivePanel:
                default:
                    ShowPanelAndActivePanel(attribute.Panel, viewController);
                    break;
            }

            if(!attribute.ShowPanel)
            {
                var menu = Mvx.Resolve<IMvxSidebarViewController>();
                menu?.CloseMenu();
            }
        }

        protected virtual bool ShowPanelAndPopToRoot(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if(navigationController == null)
                return false;

            navigationController.PopToRootViewController(false);
            navigationController.PushViewController(viewController, false);

            return true;
        }

        protected virtual bool ShowPanelAndResetToRoot(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            if(navigationController == null)
                return false;

            navigationController.ViewControllers = new[] { viewController };

            if(panel == MvxPanelEnum.Center)
                viewController.ShowMenuButton((SideBarViewController as MvxSidebarViewController));

            return true;
        }

        protected virtual bool ShowPanelAndActivePanel(MvxPanelEnum panel, UIViewController viewController)
        {
            var navigationController = (SideBarViewController as MvxSidebarViewController).NavigationController;

            switch(panel)
            {
                case MvxPanelEnum.Left:
                case MvxPanelEnum.Right:
                    break;
                case MvxPanelEnum.Center:
                default:
                    navigationController?.PushViewController(viewController, true);
                    break;
            }

            return true;
        }

        protected override void ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute, MvxViewModelRequest request)
        {
            // check if viewController is a MvxSidebarPanelController
            if(viewController is IMvxSidebarViewController)
            {
                MasterNavigationController = new MvxNavigationController();

                SideBarViewController = viewController as IMvxSidebarViewController;
                SideBarViewController.SetNavigationController(MasterNavigationController);

                SetWindowRootViewController(viewController);

                Mvx.RegisterSingleton<IMvxSidebarViewController>(SideBarViewController);

                CloseMasterNavigationController();
                CloseModalNavigationController();
                CloseSplitViewController();

                return;
            }
            else
            {
                SideBarViewController = null;
                MasterNavigationController = null;
            }

            base.ShowRootViewController(viewController, attribute, request);
        }

        public override void Close(IMvxViewModel toClose)
        {
            // if the current root is a SideBarViewController, delegate close responsibility to it
            if(SideBarViewController != null && SideBarViewController.CloseChildViewModel(toClose))
                return;

            base.Close(toClose);
        }
    }
}
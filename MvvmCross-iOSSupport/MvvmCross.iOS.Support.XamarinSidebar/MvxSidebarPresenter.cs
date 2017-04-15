namespace MvvmCross.iOS.Support.XamarinSidebar
{
    using Core.ViewModels;
    using Hints;
    using iOS.Views;
    using iOS.Views.Presenters;
    using MvvmCross.iOS.Support.XamarinSidebar.Attributes;
    using MvvmCross.iOS.Views.Presenters.Attributes;
    using MvvmCross.Platform;
    using SidePanels;
    using UIKit;

    public class MvxSidebarPresenter : MvxIosViewPresenter
    {
        protected virtual IMvxSidebarViewController SideBarViewController { get; set; }

        public MvxSidebarPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
                    : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<MvxSidebarActivePanelPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarPopToRootPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarResetRootPresentationHint>(PresentationHintHandler);
        }

        protected virtual bool PresentationHintHandler(MvxPanelPresentationHint hint)
        {
            if (hint == null)
                return false;

            hint.Navigate();

            return true;
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
            if (SideBarViewController == null)
                ShowRootViewController(new MvxSidebarViewController(), null, request);

            switch (attribute.HintType)
            {
                case MvxPanelHintType.PopToRoot:
                    ChangePresentation(new MvxSidebarPopToRootPresentationHint(attribute.Panel, SideBarViewController, viewController));
                    break;
                case MvxPanelHintType.ResetRoot:

                    ChangePresentation(new MvxSidebarResetRootPresentationHint(attribute.Panel, SideBarViewController, viewController));
                    break;
                case MvxPanelHintType.ActivePanel:
                default:

                    ChangePresentation(new MvxSidebarActivePanelPresentationHint(attribute.Panel, SideBarViewController, viewController));
                    break;
            }

            if (!attribute.ShowPanel)
            {
                var menu = Mvx.Resolve<IMvxSidebarViewController>();
                menu?.CloseMenu();
            }
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

                CloseMasterNavigationController();
                CloseModalNavigationController();
                CloseSplitViewController();

                return;
            }

            base.ShowRootViewController(viewController, attribute, request);
        }

        public override void Close(IMvxViewModel toClose)
        {
            // if the current root is a SideBarViewController, delegate close responsibility to it
            if (SideBarViewController != null && SideBarViewController.CloseChildViewModel(toClose))
                return;

            base.Close(toClose);
        }
    }
}
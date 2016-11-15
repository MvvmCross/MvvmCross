namespace MvvmCross.iOS.Support.XamarinSidebar
{
    using Core.ViewModels;
    using iOS.Views;
    using iOS.Views.Presenters;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using System.Linq;
    using UIKit;
    using SidePanels;
    using Hints;

    public class MvxSidebarPresenter : MvxIosViewPresenter
    {
        protected virtual UINavigationController ParentRootViewController { get; set; }
        protected virtual MvxSidebarPanelController RootViewController { get; set; }

        public MvxSidebarPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<MvxSidebarActivePanelPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarPopToRootPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarResetRootPresentationHint>(PresentationHintHandler);
        }

        private bool PresentationHintHandler(MvxPanelPresentationHint hint)
        {
            if (hint == null)
                return false;

            hint.Navigate();

            return true;
        }

        public override void Show(MvxViewModelRequest request)
        {
            IMvxIosView viewController = Mvx.Resolve<IMvxIosViewCreator>().CreateView(request);
            Show(viewController);
        }

        public override void Show(IMvxIosView view)
        {
            if (view is IMvxModalIosView)
            {
                PresentModalViewController(view as UIViewController, true);
                return;
            }

            var viewController = view as UIViewController;

            if (viewController == null)
                throw new MvxException("Passed in IMvxIosView is not a UIViewController");

            if (RootViewController == null)
            {
                InitRootViewController();
            }

            var viewPresentationAttribute = GetViewPresentationAttribute(view);

            //Create fall back viewPresentationAttribute, when nothing is set
            if (viewPresentationAttribute == null)
            {
                ParentRootViewController.PushViewController(viewController, ParentRootViewController.ViewControllers.Count() > 1);
                return;
            }

            switch (viewPresentationAttribute.HintType)
            {
                case MvxPanelHintType.PopToRoot:
                    ChangePresentation(new MvxSidebarPopToRootPresentationHint(viewPresentationAttribute.Panel, RootViewController, viewController));
                    break;
                case MvxPanelHintType.ResetRoot:
                    ChangePresentation(new MvxSidebarResetRootPresentationHint(viewPresentationAttribute.Panel, RootViewController, viewController));
                    break;
                case MvxPanelHintType.ActivePanel:
                default:
                    ChangePresentation(new MvxSidebarActivePanelPresentationHint(viewPresentationAttribute.Panel, RootViewController, viewController));
                    break;
            }

            if (!viewPresentationAttribute.ShowPanel)
            {
                var menu = Mvx.Resolve<IMvxSideMenu>();
                menu?.Close();
            }
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (ParentRootViewController.ViewControllers.Count() > 1)
                ParentRootViewController.PopViewController(true);
            else if (RootViewController.NavigationController.ViewControllers.Count() > 1)
                RootViewController.NavigationController.PopViewController(true);
            else
                base.Close(toClose);
        }

        protected MvxPanelPresentationAttribute GetViewPresentationAttribute(IMvxIosView view)
        {
            if (view == null)
                return default(MvxPanelPresentationAttribute);

            return view.GetType().GetCustomAttributes(typeof(MvxPanelPresentationAttribute), true).FirstOrDefault() as MvxPanelPresentationAttribute;
        }

        protected virtual void InitRootViewController()
        {
            foreach (var view in Window.Subviews)
                view.RemoveFromSuperview();

            MasterNavigationController = new UINavigationController();

            OnMasterNavigationControllerCreated();

            RootViewController = new MvxSidebarPanelController(MasterNavigationController);
            RootViewController.Initialize();

            ParentRootViewController = new UINavigationController(RootViewController);
            ParentRootViewController.NavigationBarHidden = true;

            SetWindowRootViewController(ParentRootViewController);

            Mvx.RegisterSingleton<IMvxSideMenu>(RootViewController);
        }
    }
}
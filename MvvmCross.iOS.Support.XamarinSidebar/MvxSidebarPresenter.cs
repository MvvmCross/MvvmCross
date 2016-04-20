using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System.Linq;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;
using MvvmCross.iOS.Support.XamarinSidebar.Hints;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPresenter : MvxIosViewPresenter, IMvxSideMenu
    {
        protected virtual MvxSidebarPanelController SidebarPanelController { get; private set;}

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

            if (this.SidebarPanelController == null)
            {
                this.InitSidebarController();
            }

            var viewPresentationAttribute = GetViewPresentationAttribute(view);

            switch (viewPresentationAttribute.HintType)
            {
				case MvxPanelHintType.PopToRoot:
                    ChangePresentation(new MvxSidebarPopToRootPresentationHint(viewPresentationAttribute.Panel, SidebarPanelController, viewController));
                    break;
                case MvxPanelHintType.ResetRoot:
                    ChangePresentation(new MvxSidebarResetRootPresentationHint(viewPresentationAttribute.Panel, SidebarPanelController, viewController));
                    break;
				case MvxPanelHintType.ActivePanel:
                    default:
                    ChangePresentation(new MvxSidebarActivePanelPresentationHint(viewPresentationAttribute.Panel, SidebarPanelController, viewController));
                    break;
            }
		}

        private MvxPanelPresentationAttribute GetViewPresentationAttribute(IMvxIosView view)
        {
            if (view == null)
                return default(MvxPanelPresentationAttribute);

            return view.GetType().GetCustomAttributes(typeof(MvxPanelPresentationAttribute), true).FirstOrDefault() as MvxPanelPresentationAttribute;
        }	

        protected virtual void InitSidebarController()
        {
            foreach (var view in Window.Subviews)
                view.RemoveFromSuperview();

            this.MasterNavigationController = new UINavigationController();

            this.OnMasterNavigationControllerCreated();

            SidebarPanelController = new MvxSidebarPanelController(MasterNavigationController);

            SetWindowRootViewController(SidebarPanelController);
        }

        public void ToggleMenu()
        {
            SidebarPanelController?.SidebarController?.ToggleMenu();
        }
    }
}
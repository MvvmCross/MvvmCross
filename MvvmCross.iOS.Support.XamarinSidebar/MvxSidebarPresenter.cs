using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System.Linq;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;
using SidebarNavigation;
using MvvmCross.iOS.Support.XamarinSidebar.Hints;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSidebarPresenter : MvxIosViewPresenter
    {
        protected virtual SidebarController SidebarController { get; private set;}

        public MvxSidebarPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<MvxSidebarPopToRootPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarActivePanelPresentationHint>(PresentationHintHandler);
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

            if (this.MasterNavigationController == null)
            {
                this.ShowFirstView(viewController);
            }

            var viewPresentationAttribute = GetViewPresentationAttribute(view);

            switch (viewPresentationAttribute.HintType)
            {
				case MvxPanelHintType.PopToRoot:
					ChangePresentation(new MvxSidebarPopToRootPresentationHint(viewPresentationAttribute.Panel, SidebarController, viewController));
                    break;
                case MvxPanelHintType.ResetRoot:
                    MasterNavigationController = null;
                    break;
				case MvxPanelHintType.ActivePanel:
                    default:
					ChangePresentation(new MvxSidebarActivePanelPresentationHint(viewPresentationAttribute.Panel, MasterNavigationController, viewController));
                    break;
            }
		}
		
        protected override void ShowFirstView(UIViewController viewController)
        {
            base.ShowFirstView(viewController);

            if (SidebarController == null)
            {
                SidebarController = CreateSidebarController(viewController);
            }
        }
            
		private MvxPanelPresentationAttribute GetViewPresentationAttribute(IMvxIosView view)
        {
            if (view == null)
				return default(MvxPanelPresentationAttribute);

			return view.GetType().GetCustomAttributes(typeof(MvxPanelPresentationAttribute), true).FirstOrDefault() as MvxPanelPresentationAttribute;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);
        }

        private SidebarController CreateSidebarController(UIViewController viewController)
        {
            var sidebarController = new SidebarController(Window.RootViewController, viewController, new UIViewController());
            sidebarController.HasShadowing = true;
            sidebarController.MenuWidth = 220;

            return sidebarController;
        }
    }
}
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System.Linq;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSideMenuPresenter : MvxIosViewPresenter
    {
		private RootViewController _rootController;

		private RootViewController RootController
        {
            get { return _rootController; }
            set
            {
                if (_rootController == value)
                    return;

                _rootController = value;
                base.SetWindowRootViewController(_rootController);
            }
        }

        public MvxSideMenuPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<MvxPanelPopToRootPresentationHint>(PopToRootPresentationHintHandler);
            AddPresentationHintHandler<MvxPanelResetRootPresentationHint>(ResetRootPresentationHintHandler);
            AddPresentationHintHandler<MvxPanelPushViewPresentationHint>(PushViewPresentationHintHandler);
        }

		private bool PopToRootPresentationHintHandler(MvxPanelPopToRootPresentationHint hint)
        {
            if (hint == null)
                return false;

            RootController.NavController.ViewControllers = null;
            RootController.NavController.PushViewController(hint.ViewController, false);
            RootController.SidebarController.CloseMenu(true);

            return true;
        }

        private bool ResetRootPresentationHintHandler(MvxPanelResetRootPresentationHint hint)
        {
            if (hint == null)
                return false;

            RootController = null;

            return true;
        }

        private bool PushViewPresentationHintHandler(MvxPanelPushViewPresentationHint hint)
        {
            if (hint == null)
                return false;

            RootController.NavController.ShowViewController(hint.ViewController, null);
            RootController.SidebarController.CloseMenu(true);

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

            if (RootController == null)
            {
                RootController = new RootViewController();
				ChangePresentation(new MvxPanelPopToRootPresentationHint(viewController));
            }
            else
            {
                var viewPresentationAttribute = GetViewPresentationAttribute(view);

                switch (viewPresentationAttribute.HintType)
                {
				case MvxPanelHintType.PopToRoot:
                        ChangePresentation(new MvxPanelPopToRootPresentationHint(viewController));
                        break;

				case MvxPanelHintType.ResetRoot:
                        ChangePresentation(new MvxPanelResetRootPresentationHint(viewController));
                        break;

				case MvxPanelHintType.PushView:
                    default:
                        ChangePresentation(new MvxPanelPushViewPresentationHint(viewController));
                        break;
                }

				switch (viewPresentationAttribute.Panel) {
				case MvxPanelEnum.Left:
					RootController.MenuController = viewController;
					//RootController.SidebarController.MenuAreaController = new UINavigationController(viewController); MenuAreaController = GET ONLY
					break;
				case MvxPanelEnum.Center:
					break;
				case MvxPanelEnum.Right:
					break;
				
				}
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

        public override void Close(IMvxViewModel toClose)
        {
            if (RootController.NavController == null)
                return;

            var mvxIosView = RootController.NavController.TopViewController as IMvxIosView;

            if (mvxIosView == null || mvxIosView.ReflectionGetViewModel() != toClose)
                return;

            RootController.NavController.PopViewController(true);
        }

        public override bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            var result = base.PresentModalViewController(viewController, animated);

            // Add swipe-down gesture to all modal ViewControllers, since you expect, that you can move them downwards, the direction they came from.
            var recognizer = new UISwipeGestureRecognizer(CloseModalViewController);
            recognizer.Direction = UISwipeGestureRecognizerDirection.Down;
            viewController.View.AddGestureRecognizer(recognizer);

            return result;
        }

        public override void CloseModalViewController()
        {
            base.CloseModalViewController();
        }
    }
}
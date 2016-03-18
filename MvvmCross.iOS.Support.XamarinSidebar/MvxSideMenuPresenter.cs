using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System.Linq;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    /*public class SideMenuPresenter : MvxIosViewPresenter
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

        public SideMenuPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<PopToRootPresentationHint>(PopToRootPresentationHintHandler);
            AddPresentationHintHandler<ResetRootPresentationHint>(ResetRootPresentationHintHandler);
            AddPresentationHintHandler<PushViewPresentationHint>(PushViewPresentationHintHandler);
        }

        private bool PopToRootPresentationHintHandler(PopToRootPresentationHint hint)
        {
            if (hint == null)
                return false;

            RootController.NavController.ViewControllers = null;
            RootController.NavController.PushViewController(hint.ViewController, false);
            RootController.SidebarController.CloseMenu(true);

            return true;
        }

        private bool ResetRootPresentationHintHandler(ResetRootPresentationHint hint)
        {
            if (hint == null)
                return false;

            RootController = null;

            return true;
        }

        private bool PushViewPresentationHintHandler(PushViewPresentationHint hint)
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
                ChangePresentation(new PopToRootPresentationHint(viewController));
            }
            else
            {
                var viewPresentationAttribute = GetViewPresentationAttribute(view);

                switch (viewPresentationAttribute.HintType)
                {
                    case PresentationHintType.PopToRoot:
                        ChangePresentation(new PopToRootPresentationHint(viewController));
                        break;

                    case PresentationHintType.ResetRoot:
                        ChangePresentation(new ResetRootPresentationHint(viewController));
                        break;

                    case PresentationHintType.PushView:
                    default:
                        ChangePresentation(new PushViewPresentationHint(viewController));
                        break;
                }
            }
        }

        private ViewPresentationAttribute GetViewPresentationAttribute(IMvxIosView view)
        {
            if (view == null)
                return default(ViewPresentationAttribute);

            return view.GetType().GetCustomAttributes(typeof(ViewPresentationAttribute), true).FirstOrDefault() as ViewPresentationAttribute;
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
    }*/
}
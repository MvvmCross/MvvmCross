using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System.Linq;
using UIKit;
using SidebarNavigation;
using MvvmCross.iOS.Support.XamarinSidebar.Hints;

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSideMenuPresenter : MvxIosViewPresenter
    {
        protected virtual SidebarController SidebarController { get; private set;}

        public MvxSideMenuPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            AddPresentationHintHandler<MvxSidebarMasterPresentationHint>(PresentationHintHandler);
            AddPresentationHintHandler<MvxSidebarDetailPresentationHint>(PresentationHintHandler);
        }

        private bool PresentationHintHandler(MvxSidebarBasePresentationHint hint)
        {
            if (hint == null)
                return false;

            hint.HandleNavigation();

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
            else
            {
                var viewPresentationAttribute = GetViewPresentationAttribute(view);
                
                if (viewPresentationAttribute.HintType == MvxSidebarHintType.Master)
                    ChangePresentation(new MvxSidebarMasterPresentationHint(viewController, SidebarController));
                else
                    ChangePresentation(new MvxSidebarDetailPresentationHint(viewController, MasterNavigationController));
            }
        }

        protected override void ShowFirstView(UIViewController viewController)
        {
            base.ShowFirstView(viewController);

            if (SidebarController == null)
            {
                SidebarController = CreateSidebarController(viewController);
            }

            AddSidebarToggle(viewController);
        }

        protected virtual void AddSidebarToggle(UIViewController viewController)
        {
            viewController.NavigationItem.SetLeftBarButtonItem(
                new UIBarButtonItem(UIImage.FromBundle("threelines")
                    , UIBarButtonItemStyle.Plain
                    , (sender, args) => SidebarController.ToggleMenu ()), true);
        }
 
        private MvxSidebarPresentationAttribute GetViewPresentationAttribute(IMvxIosView view)
        {
            if (view == null)
                return default(MvxSidebarPresentationAttribute);

            return view.GetType().GetCustomAttributes(typeof(MvxSidebarPresentationAttribute), true).FirstOrDefault() as MvxSidebarPresentationAttribute;
        }       

        private SidebarController CreateSidebarController(UIViewController viewController)
        {
            var sidebarController = new SidebarController(Window.RootViewController, viewController, new UIViewController());
            sidebarController.HasShadowing = true;
            sidebarController.MenuWidth = 220;
            sidebarController.MenuLocation = SidebarNavigation.MenuLocations.Left;

            return sidebarController;
        }
    }
}
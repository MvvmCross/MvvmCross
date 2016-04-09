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

namespace MvvmCross.iOS.Support.XamarinSidebar
{
    public class MvxSideMenuPresenter : MvxIosViewPresenter
    {
        protected virtual SidebarController SidebarController { get; private set;}

        public MvxSideMenuPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
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
                this.ShowFirstView(viewController);
            else
                this.MasterNavigationController.PushViewController(viewController, true /*animated*/);
        }

        protected override void ShowFirstView(UIViewController viewController)
        {
            base.ShowFirstView(viewController);

            if (SidebarController == null)
            {
                SidebarController = CreateSidebarController(viewController);

                viewController.NavigationItem.SetLeftBarButtonItem(
                    new UIBarButtonItem(UIImage.FromBundle("threelines")
                        , UIBarButtonItemStyle.Plain
                        , (sender, args) => SidebarController.ToggleMenu ()), true);
            }
        }

        private SidebarController CreateSidebarController(UIViewController viewController)
        {
            var sidebarController = new SidebarController(Window.RootViewController, viewController, new UIViewController());
            sidebarController.HasShadowing = false;
            sidebarController.MenuWidth = 220;
            sidebarController.MenuLocation = SidebarNavigation.MenuLocations.Left;

            return sidebarController;
        }
    }
}
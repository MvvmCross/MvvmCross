using System;
using MvvmCross.Core.ViewModels;
using UIKit;
using MvvmCross.iOS.Support.SidePanels;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarActivePanelPresentationHint : MvxPanelPresentationHint
    {
        public MvxSidebarActivePanelPresentationHint(MvxPanelEnum panel, UINavigationController navigationController, UIViewController viewController)
            : base(panel)
        {
            NavigationController = navigationController;
            ViewController = viewController;
        }

        protected readonly UINavigationController NavigationController;
        protected readonly UIViewController ViewController;

        public override bool Navigate()
        {
            if (ViewController == null || NavigationController == null)
                return false;

            NavigationController.PushViewController(ViewController, true);

            return true;
        }
    }
}


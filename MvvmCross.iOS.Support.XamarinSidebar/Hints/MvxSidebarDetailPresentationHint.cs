using System;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public class MvxSidebarDetailPresentationHint : MvxSidebarBasePresentationHint
    {
        private readonly UINavigationController _navigationController;

        public MvxSidebarDetailPresentationHint(UIViewController viewController, UINavigationController navigationController)
            : base(viewController)
        {
            _navigationController = navigationController;
        }

        public override bool HandleNavigation()
        {
            if (ViewController == null || _navigationController == null)
                return false;

            _navigationController.PushViewController(ViewController, true);

            return true;
        }
    }
}


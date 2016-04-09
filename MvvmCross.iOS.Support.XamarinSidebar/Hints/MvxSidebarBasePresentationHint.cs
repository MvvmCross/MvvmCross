using System;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.XamarinSidebar.Hints
{
    public abstract class MvxSidebarBasePresentationHint : MvxPresentationHint
    {
        public MvxSidebarBasePresentationHint(UIViewController viewController)
        {
            ViewController = viewController;
        }

        protected readonly UIViewController ViewController;

        public abstract bool HandleNavigation();
    }
}


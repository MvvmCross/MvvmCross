// MvxModalNavSupportIosViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.iOS.Views.Presenters
{
    public class MvxModalNavSupportIosViewPresenter : MvxIosViewPresenter
    {
        private UIViewController _currentModalViewController;

        public MvxModalNavSupportIosViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public override void Show(IMvxIosView view)
        {
            if(view is IMvxModalIosView)
            {
                if(_currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                var newNav = CreateModalNavigationController();
                newNav.PushViewController(view as UIViewController, false);

                _currentModalViewController = view as UIViewController;

                PresentModalViewController(newNav, true);
                return;
            }

            base.Show(view);
        }

        protected virtual UINavigationController CreateModalNavigationController()
        {
            var newNav = new UINavigationController();
            return newNav;
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            if(_currentModalViewController != null)
            {
                MvxTrace.Error("How did a modal disappear when we didn't have one showing?");
                return;
            }

            // clear our local reference to avoid back confusion
            _currentModalViewController = null;
        }

        public override void CloseModalViewController()
        {
            if(_currentModalViewController != null)
            {
                var nav = _currentModalViewController.ParentViewController as UINavigationController;
                if(nav != null)
                    nav.DismissViewController(true, () => { });
                else
                    _currentModalViewController.DismissViewController(true, () => { });
                _currentModalViewController = null;
                return;
            }

            base.CloseModalViewController();
        }

        public override void Close(IMvxViewModel toClose)
        {
            if(_currentModalViewController != null)
            {
                var touchView = _currentModalViewController as IMvxIosView;
                if(touchView == null)
                {
                    MvxTrace.Error("Unable to close view - modal is showing but not an IMvxIosView");
                    return;
                }

                var viewModel = touchView.ReflectionGetViewModel();
                if(viewModel != toClose)
                {
                    MvxTrace.Error("Unable to close view - modal is showing but is not the requested viewmodel");
                    return;
                }

                var nav = _currentModalViewController.ParentViewController as UINavigationController;
                if(nav != null)
                    nav.DismissViewController(true, () => { });
                else
                    _currentModalViewController.DismissViewController(true, () => { });
                _currentModalViewController = null;
                return;
            }

            base.Close(toClose);
        }
    }
}
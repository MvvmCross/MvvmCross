// MvxModalNavSupportTouchViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views.Presenters
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxModalNavSupportTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIViewController _currentModalViewController;

        public MvxModalNavSupportTouchViewPresenter(IUIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public override void Show(IMvxTouchView view)
        {
            if (view is IMvxModalTouchView)
            {
                if (this._currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                var newNav = this.CreateModalNavigationController();
                newNav.PushViewController(view as UIViewController, false);

                this._currentModalViewController = view as UIViewController;

                this.PresentModalViewController(newNav, true);
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
            if (this._currentModalViewController != null)
            {
                MvxTrace.Error("How did a modal disappear when we didn't have one showing?");
                return;
            }

            // clear our local reference to avoid back confusion
            this._currentModalViewController = null;
        }

        public override void CloseModalViewController()
        {
            if (this._currentModalViewController != null)
            {
                var nav = this._currentModalViewController.ParentViewController as UINavigationController;
                if (nav != null)
                    nav.DismissViewController(true, () => { });
                else
                    this._currentModalViewController.DismissViewController(true, () => { });
                this._currentModalViewController = null;
                return;
            }

            base.CloseModalViewController();
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (this._currentModalViewController != null)
            {
                var touchView = this._currentModalViewController as IMvxTouchView;
                if (touchView == null)
                {
                    MvxTrace.Error(
                                   "Unable to close view - modal is showing but not an IMvxTouchView");
                    return;
                }

                var viewModel = touchView.ReflectionGetViewModel();
                if (viewModel != toClose)
                {
                    MvxTrace.Error(
                                   "Unable to close view - modal is showing but is not the requested viewmodel");
                    return;
                }

                var nav = this._currentModalViewController.ParentViewController as UINavigationController;
                if (nav != null)
                    nav.DismissViewController(true, () => { });
                else
                    this._currentModalViewController.DismissViewController(true, () => { });
                this._currentModalViewController = null;
                return;
            }

            base.Close(toClose);
        }
    }
}
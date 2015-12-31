// MvxModalSupportTouchViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views.Presenters
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxModalSupportTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIViewController _currentModalViewController;

        public MvxModalSupportTouchViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public override void Show(IMvxTouchView view)
        {
            if (view is IMvxModalTouchView)
            {
                if (this._currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                this._currentModalViewController = view as UIViewController;

                this.PresentModalViewController(view as UIViewController, true);
                return;
            }

            base.Show(view);
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
                this._currentModalViewController.DismissModalViewController(true);
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

                this._currentModalViewController.DismissViewController(true, () => { });
                this._currentModalViewController = null;
                return;
            }

            base.Close(toClose);
        }
    }
}
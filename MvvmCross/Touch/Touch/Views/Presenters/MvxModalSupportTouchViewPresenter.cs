// MvxModalSupportTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
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
                if (_currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                _currentModalViewController = view as UIViewController;

                PresentModalViewController(view as UIViewController, true);
                return;
            }

            base.Show(view);
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            if (_currentModalViewController != null)
            {
                MvxTrace.Error("How did a modal disappear when we didn't have one showing?");
                return;
            }

            // clear our local reference to avoid back confusion
            _currentModalViewController = null;
        }

        public override void CloseModalViewController()
        {
            if (_currentModalViewController != null)
            {
                _currentModalViewController.DismissModalViewController(true);
                _currentModalViewController = null;
                return;
            }

            base.CloseModalViewController();
        }

        public override void Close(IMvxViewModel toClose)
        {
            if (_currentModalViewController != null)
            {
                var touchView = _currentModalViewController as IMvxTouchView;
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

                _currentModalViewController.DismissViewController(true, () => { });
                _currentModalViewController = null;
                return;
            }

            base.Close(toClose);
        }
    }
}
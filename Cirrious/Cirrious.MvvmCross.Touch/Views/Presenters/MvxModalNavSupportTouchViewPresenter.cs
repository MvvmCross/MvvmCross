// MvxModalNavSupportTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxModalNavSupportTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIViewController _currentModalViewController;

        public MvxModalNavSupportTouchViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        public override void Show(IMvxTouchView view)
        {
            if (view is IMvxModalTouchView)
            {
                if (_currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                var newNav = new UINavigationController();
                newNav.PushViewController(view as UIViewController, false);

                _currentModalViewController = view as UIViewController;

                PresentModalViewController(newNav, true);
                return;
            }

            base.Show(view);
        }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            if (_currentModalViewController != null)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, "How did a modal disappear when we didn't have one showing?");
                return;
            }

            // clear our local reference to avoid back confusion
            _currentModalViewController = null;
        }

        public override void CloseModalViewController()
        {
            if (_currentModalViewController != null)
            {
                var nav = _currentModalViewController.ParentViewController as UINavigationController;
                if (nav != null)
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
            if (_currentModalViewController != null)
            {
                var touchView = _currentModalViewController as IMvxTouchView;
                if (touchView == null)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error,
                                   "Unable to close view - modal is showing but not an IMvxTouchView");
                    return;
                }

                var viewModel = touchView.ReflectionGetViewModel();
                if (viewModel != toClose)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error,
                                   "Unable to close view - modal is showing but is not the requested viewmodel");
                    return;
                }

                var nav = _currentModalViewController.ParentViewController as UINavigationController;
                if (nav != null)
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
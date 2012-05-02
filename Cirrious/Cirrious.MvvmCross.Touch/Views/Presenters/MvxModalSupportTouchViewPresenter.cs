#region Copyright
// <copyright file="MvxModalSupportTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxModalSupportTouchViewPresenter : MvxTouchViewPresenter
    {
        private UIViewController _currentModalViewController;

        public MvxModalSupportTouchViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window)
            : base (applicationDelegate, window)
        {
        } 
    
        public override void Show (IMvxTouchView view)
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
                _currentModalViewController.DismissModalViewControllerAnimated(true);
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
                    MvxTrace.Trace(MvxTraceLevel.Error, "Unable to close view - modal is showing but not an IMvxTouchView");
                    return;
                }

                var viewModel = touchView.ReflectionGetViewModel();
                if (viewModel != toClose)
                {
                    MvxTrace.Trace(MvxTraceLevel.Error, "Unable to close view - modal is showing but is not the requested viewmodel");
                    return;
                }

                _currentModalViewController.DismissModalViewControllerAnimated(true);
                _currentModalViewController = null;
                return;
            }

            base.Close(toClose);
        }
    }	
}

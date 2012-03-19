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
using Cirrious.MvvmCross.Touch.Interfaces;
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
	
		public override bool ShowView (IMvxTouchView view)
		{
            if (view is IMvxModalTouchView)
            {
                if (_currentModalViewController != null)
                    throw new MvxException("Only one modal view controller at a time supported");

                _currentModalViewController = view as UIViewController;

                PresentNativeModalViewController(view as UIViewController, true);
                return true;
            }

		    return base.ShowView(view);
		}

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // clear our local reference to avoid back confusion
            _currentModalViewController = null;
        }

		public override bool GoBack ()
		{
			if (_currentModalViewController != null)
			{
				_currentModalViewController.DismissModalViewControllerAnimated (true);
				_currentModalViewController = null;
				return true;
			}

		    return base.GoBack();
		}
	}	
}

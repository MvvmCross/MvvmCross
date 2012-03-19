#region Copyright
// <copyright file="MvxTouchViewPresenter.cs" company="Cirrious">
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
    public class MvxTouchViewPresenter : MvxBaseTouchViewPresenter
	{
		private readonly UIApplicationDelegate _applicationDelegate;
		private readonly UIWindow _window;
		
		private UINavigationController _masterNavigationController;
		
		public MvxTouchViewPresenter (UIApplicationDelegate applicationDelegate, UIWindow window)
		{
			_applicationDelegate = applicationDelegate;
			_window = window;
		} 
			
		public override bool ShowView (IMvxTouchView view)
		{			
			var viewController = view as UIViewController;
			if (viewController == null)
				throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
			
			if (_masterNavigationController == null)
				ShowFirstView(viewController);
			else
                _masterNavigationController.PushViewController(viewController, true /*animated*/);
		
            return true;
		}

        public override bool GoBack()
		{
			_masterNavigationController.PopViewControllerAnimated(true);
		    return true;
		}

        public override void ClearBackStack()
        {
			if (_masterNavigationController == null)
				return;
			
            _masterNavigationController.PopToRootViewController (true);
			_masterNavigationController = null;
        }

        public override bool PresentNativeModalViewController(UIViewController viewController, bool animated)
	    {
            CurrentTopViewController.PresentModalViewController(viewController, animated);
            return true;
	    }

        public override void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // ignored
        }

        protected virtual void ShowFirstView (UIViewController viewController)
		{
			foreach (var view in _window.Subviews)
				view.RemoveFromSuperview();
			
			_masterNavigationController = CreateNavigationController(viewController);

            OnMasterNavigationControllerCreated();

            _window.AddSubview(_masterNavigationController.View);
	    }
		
		protected virtual void OnMasterNavigationControllerCreated ()
		{
		}
		
		protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
		{
			return new UINavigationController(viewController);			
		}

        protected virtual UIViewController CurrentTopViewController
        {
            get { return _masterNavigationController.TopViewController; }
        }
	}	
}

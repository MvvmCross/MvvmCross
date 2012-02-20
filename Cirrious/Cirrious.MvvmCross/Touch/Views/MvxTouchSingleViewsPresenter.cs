#region Copyright

// <copyright file="MvxTouchSingleViewsContainer.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Linq;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Platform.Diagnostics;


namespace Cirrious.MvvmCross.Touch.Views
{
#warning Want to split this out into separate folders in separate implementations - sooner the better
#warning Fundamentally I think MvxTouchViewDisplayType should be optional and should be off in a subfolder
    public class MvxTouchSingleViewsPresenter : IMvxTouchViewPresenter
	{
		private readonly UIApplicationDelegate _applicationDelegate;
		private readonly UIWindow _window;
		
		private UINavigationController _masterNavigationController;
#warning Stuart still wants to try to understand what iOS does under the covers here (to prevent stacks of modals)
        private UIViewController _currentModalViewController;
		
		public MvxTouchSingleViewsPresenter (UIApplicationDelegate applicationDelegate, UIWindow window)
		{
			_applicationDelegate = applicationDelegate;
			_window = window;
		} 
		
	
		public virtual void ShowView (IMvxTouchView view)
		{			
			var viewController = view as UIViewController;
			if (viewController == null)
				throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
			
			if (_masterNavigationController == null)
				ShowFirstView(viewController);
			else
			{
                if (view.DisplayType == MvxTouchViewDisplayType.Master)
                    _masterNavigationController.PushViewController(viewController, true /*animated*/);
                else
				{					
                    PresentNativeModalViewController (viewController, true /*animated*/);
				}
			}				 
		}
		
		public void GoBack ()
		{
			if (_currentModalViewController != null)
			{
				_currentModalViewController.DismissModalViewControllerAnimated (true);
				
				_currentModalViewController = null;
				
				return;
			}
			
			_masterNavigationController.PopViewControllerAnimated(true);
		}
		
		public void ClearBackStack ()
        {
			if (_masterNavigationController == null)
				return;
			
#warning Why is all this necessary? I need to ask Pavel, answ: So the navigation stack does not get corrupted, exchanging the collection of view controllers and not poping first led to errors		
            _masterNavigationController.PopToRootViewController (true);
			_masterNavigationController.ViewControllers = new UIViewController [] {};
			_masterNavigationController = null;
        }

        public void PresentNativeModalViewController(UIViewController viewController, bool animated)
	    {
			_currentModalViewController = viewController;
            _masterNavigationController.TopViewController.PresentModalViewController(viewController, animated);
        }

	    private void ShowFirstView (UIViewController viewController)
		{
			foreach (var view in _window.Subviews)
				view.RemoveFromSuperview();
			
			_masterNavigationController = CreateNavigationController(viewController);			
			
			_window.AddSubview (_masterNavigationController.View);
			_window.MakeKeyAndVisible();
			
			OnCreated ();
		}
		
		protected virtual void OnCreated ()
		{
		}
		
		protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
		{
			return new UINavigationController(viewController);			
		}
	}	
}

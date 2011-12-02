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


namespace Cirrious.MvvmCross.Touch.Views
{
	public class MvxTouchSingleViewsPresenter : IMvxTouchViewPresenter
	{
		private readonly UIApplicationDelegate _applicationDelegate;
		private readonly UIWindow _window;
		
		private UINavigationController _masterNavigationController;

		public MvxTouchSingleViewsPresenter (UIApplicationDelegate applicationDelegate, UIWindow window)
		{
			_applicationDelegate = applicationDelegate;
			_window = window;
		} 
		
		public void ShowView(IMvxTouchView view)
		{			
			var viewController = view as UIViewController;
			if (viewController == null)
				throw new MvxException("Passed in IMvxTouchView is not a UIViewController");
			
			if (_masterNavigationController == null)
				ShowFirstView(viewController);
			else
				_masterNavigationController.PushViewController(viewController, true /* animate */);
		}
		
		public void GoBack()
		{
			_masterNavigationController.PopViewControllerAnimated(true);
		}
		
		private void ShowFirstView(UIViewController viewController)
		{
			_masterNavigationController = new UINavigationController(viewController);			
			foreach (var view in _window.Subviews)
				view.RemoveFromSuperview();
			
			_window.Add(_masterNavigationController.View);
			_window.MakeKeyAndVisible();
		}
	}	
}

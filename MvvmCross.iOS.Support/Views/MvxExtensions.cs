using System;
using MvvmCross.iOS.Views;
using MvvmCross.Platform.Exceptions;
using UIKit;

namespace MvvmCross.iOS.Support.Views
{
	public static class MvxExtensions
	{
		public static IMvxIosView GetIMvxIosView(this UIViewController viewController)
		{
			var mvxView = viewController as IMvxIosView;
			if(mvxView == null)
			{
				throw new MvxException("Could not get IMvxIosView from ViewController!");
			}
			return mvxView;
		}
	}
}


using System;
using UIKit;

namespace MvvmCross.iOS.Support.Presenters
{
	public interface IMvxTabBarPresenter
	{
		IMvxTabBarViewController TabBarViewController { get; set; }

		UINavigationController NavigationController { get; set; }
	}
}


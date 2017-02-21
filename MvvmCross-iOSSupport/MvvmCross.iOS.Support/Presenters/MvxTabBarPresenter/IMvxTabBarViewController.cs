﻿using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.Presenters
{
	public interface IMvxTabBarViewController
	{
		void ShowTabView(UIViewController viewController, bool wrapInNavigationController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null);

		bool ShowChildView(UIViewController viewController);

		bool CloseChildViewModel(IMvxViewModel viewModel);
	}
}


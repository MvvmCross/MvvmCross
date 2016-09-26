using System;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Support.Presenters
{
	public interface IMvxTabBarViewController
	{
		void ShowTabView(UIViewController viewController, bool wrapInNavigationController, IDictionary<string, string> presentationValues);

		bool ShowChildView(UIViewController viewController);

		bool CloseChildViewModel(IMvxViewModel viewModel);
	}
}


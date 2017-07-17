﻿using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabSelectedIconName = null, string tabAccessibilityIdentifier = null);

        void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(IMvxViewModel viewModel);

        bool CanShowChildView(UIViewController viewController);
    }
}

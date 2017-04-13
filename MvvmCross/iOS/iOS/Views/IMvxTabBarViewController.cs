using System;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace MvvmCross.iOS.Views
{
    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, string tabTitle, string tabIconName, string tabAccessibilityIdentifier = null);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(IMvxViewModel viewModel);
    }
}

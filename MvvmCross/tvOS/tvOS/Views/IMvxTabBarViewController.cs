using MvvmCross.Core.ViewModels;
using MvvmCross.tvOS.Views.Presenters.Attributes;
using UIKit;

namespace MvvmCross.tvOS.Views
{
    public interface IMvxTabBarViewController
    {
        void ShowTabView(UIViewController viewController, MvxTabPresentationAttribute attribute);

        bool ShowChildView(UIViewController viewController);

        bool CloseChildViewModel(IMvxViewModel viewModel);

        bool CloseTabViewModel(IMvxViewModel viewModel);

        bool CanShowChildView();
    }
}

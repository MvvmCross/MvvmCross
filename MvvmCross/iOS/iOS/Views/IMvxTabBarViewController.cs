using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace MvvmCross.iOS.Views
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

using UIKit;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, bool wrapInNavigationController);

        void ShowDetailView(UIViewController viewController, bool wrapInNavigationController);

        bool CloseChildViewModel(IMvxViewModel viewModel);
    }
}

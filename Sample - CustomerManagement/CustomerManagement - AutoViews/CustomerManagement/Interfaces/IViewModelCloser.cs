using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.AutoViews.Core.Interfaces
{
    public interface IViewModelCloser
    {
        void RequestClose(IMvxViewModel viewModel);
    }
}
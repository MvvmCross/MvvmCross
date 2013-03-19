using Cirrious.MvvmCross.ViewModels;

namespace CustomerManagement.Core.Interfaces
{
    public interface IViewModelCloser
    {
        void RequestClose(IMvxViewModel viewModel);
    }
}
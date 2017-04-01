using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Views
{
    public interface IMvxNavModalIosView
        : IMvxModalIosView
    {
    }

    public interface IMvxNavModalIosView<TViewModel> : IMvxModalIosView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Core
{
    public interface IMvxContentPage : IMvxView
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxContentPage<TViewModel>
        : IMvxContentPage
    , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}


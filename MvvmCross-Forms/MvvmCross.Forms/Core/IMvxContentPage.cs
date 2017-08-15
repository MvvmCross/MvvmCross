using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Core
{
    public interface IMvxContentPage : IMvxView, IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxContentPage<TViewModel>
        : IMvxContentPage
    , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
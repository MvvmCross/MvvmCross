using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxContentPage : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxContentPage<TViewModel>
        : IMvxContentPage
    , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
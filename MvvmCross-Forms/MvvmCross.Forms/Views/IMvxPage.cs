using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxPage : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxPage<TViewModel>
        : IMvxPage, IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
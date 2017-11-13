using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxPage : IMvxElement
    {
    }

    public interface IMvxPage<TViewModel>
        : IMvxPage, IMvxElement<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
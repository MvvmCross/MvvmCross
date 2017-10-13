using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxPage : IMvxFormsView
    {
    }

    public interface IMvxPage<TViewModel>
        : IMvxPage, IMvxFormsView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
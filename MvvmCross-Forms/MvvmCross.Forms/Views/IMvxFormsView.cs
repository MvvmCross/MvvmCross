using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxFormsView : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxFormsView<TViewModel>
        : IMvxFormsView, IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}

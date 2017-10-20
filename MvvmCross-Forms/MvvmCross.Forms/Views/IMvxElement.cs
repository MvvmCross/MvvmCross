using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Forms.Views
{
    public interface IMvxElement : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxElement<TViewModel>
        : IMvxElement, IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}

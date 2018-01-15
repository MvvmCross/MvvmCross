// IMvxIosView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views
{
    public interface IMvxIosView
        : IMvxView
        , IMvxCanCreateIosView
        , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxIosView<TViewModel>
        : IMvxIosView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
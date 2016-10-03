// IMvxTvosView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxTvosView
        : IMvxView
        , IMvxCanCreateTvosView
        , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxTvosView<TViewModel>
        : IMvxTvosView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
// IMvxTouchView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Touch.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxTouchView
        : IMvxView
        , IMvxCanCreateTouchView
        , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxTouchView<TViewModel>
        : IMvxTouchView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}
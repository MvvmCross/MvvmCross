// IMvxFragmentView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Droid.Shared.Fragments
{
    public interface IMvxFragmentView
        : IMvxBindingContextOwner
        , IMvxView
    {
        string UniqueImmutableCacheTag { get; }
    }

    public interface IMvxFragmentView<TViewModel>
        : IMvxFragmentView
        , IMvxView<TViewModel> where TViewModel : class
        , IMvxViewModel
    {
    }
}
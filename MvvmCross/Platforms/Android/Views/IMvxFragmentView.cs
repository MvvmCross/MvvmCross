﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
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
        MvxFluentBindingDescriptionSet<IMvxFragmentView<TViewModel>, TViewModel> CreateBindingSet();
    }
}

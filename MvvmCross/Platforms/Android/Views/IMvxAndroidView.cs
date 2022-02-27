// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public interface IMvxAndroidView
        : IMvxView
        , IMvxLayoutInflaterHolder
        , IMvxStartActivityForResult
        , IMvxBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : IMvxAndroidView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxAndroidView<TViewModel>, TViewModel> CreateBindingSet();
    }
}

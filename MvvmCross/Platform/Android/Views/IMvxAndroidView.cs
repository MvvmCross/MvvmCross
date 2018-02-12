// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Android.Views.Base;
using MvvmCross.Platform.Android.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platform.Android.Views
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
    }
}

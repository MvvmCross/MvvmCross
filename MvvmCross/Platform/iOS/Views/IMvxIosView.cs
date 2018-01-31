// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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
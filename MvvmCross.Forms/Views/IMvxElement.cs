// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;

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

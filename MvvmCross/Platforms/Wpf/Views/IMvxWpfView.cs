// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Wpf.Views
{
    public interface IMvxWpfView
        : IMvxView, IMvxBindingContextOwner
    {
    }

    public interface IMvxWpfView<TViewModel>
        : IMvxWpfView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        MvxFluentBindingDescriptionSet<IMvxWpfView<TViewModel>, TViewModel> CreateBindingSet();
    }
}

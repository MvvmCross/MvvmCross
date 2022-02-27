// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Tvos.Views
{
    public interface IMvxModalTvosView
        : IMvxView
    {
    }

    public interface IMvxModalTvosView<TViewModel>
        : IMvxModalTvosView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}

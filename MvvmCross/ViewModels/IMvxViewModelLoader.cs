// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
    public interface IMvxViewModelLoader
    {
        IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel LoadViewModel<TParameter>(MvxViewModelRequest request, TParameter param, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel ReloadViewModel<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, MvxViewModelRequest request, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);
    }
}

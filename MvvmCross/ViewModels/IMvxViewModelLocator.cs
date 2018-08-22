// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Navigation.EventArguments;

namespace MvvmCross.ViewModels
{
    public interface IMvxViewModelLocator
    {
        IMvxViewModel Load(Type viewModelType, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel<TParameter> Load<TParameter>(Type viewModelType, TParameter param, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel Reload(IMvxViewModel viewModel, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);

        IMvxViewModel<TParameter> Reload<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle parameterValues, IMvxBundle savedState, IMvxNavigateEventArgs navigationArgs = null);
    }
}

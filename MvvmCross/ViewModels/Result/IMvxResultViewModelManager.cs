// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public interface IMvxResultViewModelManager
{
    void RegisterToResult<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel);

    bool UnregisterToResult<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel);

    bool IsRegistered<TResult>(IMvxResultAwaitingViewModel<TResult> viewModel);

    void SetResult<TResult>(IMvxResultSettingViewModel<TResult> viewModel, TResult result);
}

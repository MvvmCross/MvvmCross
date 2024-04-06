// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public static class MvxResultViewModelExtensions
{
    public const string BundleRegisterKey = "__mvxResultVMRegisterKey";

    public static void ReloadAndRegisterToResult<TResult>(this IMvxResultAwaitingViewModel<TResult> viewModel, IMvxBundle savedStateBundle)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxResultViewModelManager>(out var manager) == true &&
            savedStateBundle?.Data?.TryGetValue(BundleRegisterKey, out string restoreRegisterStr) == true &&
            bool.TryParse(restoreRegisterStr, out bool restoreRegister) && restoreRegister)
        {
            manager.RegisterToResult(viewModel);
        }
    }

    public static void SaveRegisterToResult<TResult>(this IMvxResultAwaitingViewModel<TResult> viewModel, IMvxBundle savedStateBundle)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxResultViewModelManager>(out var manager) == true &&
            manager.IsRegistered(viewModel) &&
            savedStateBundle?.Data is { } data)
        {
            data[BundleRegisterKey] = true.ToString();
        }
    }

    public static void RegisterToResult<TResult>(this IMvxResultAwaitingViewModel<TResult> viewModel)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxResultViewModelManager>(out var manager) == true)
        {
            manager.RegisterToResult(viewModel);
        }
    }

    public static void UnregisterToResult<TResult>(this IMvxResultAwaitingViewModel<TResult> viewModel)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxResultViewModelManager>(out var manager) == true)
        {
            manager.UnregisterToResult(viewModel);
        }
    }

    public static void SetResult<TResult>(this IMvxResultSettingViewModel<TResult> viewModel, TResult result)
    {
        if (Mvx.IoCProvider?.TryResolve<IMvxResultViewModelManager>(out var manager) == true)
        {
            manager.SetResult(viewModel, result);
        }
    }
}

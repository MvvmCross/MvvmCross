// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public static class MvxResultViewModelExtensions
{
    public const string BundleRegisterKey = "__mvxResultVMRegisterKey";

    public static void ReloadAndRegisterToResult<TResult>(
        this IMvxResultAwaitingViewModel<TResult> viewModel,
        IMvxBundle savedStateBundle,
        IMvxResultViewModelManager resultViewModelManager)
    {
        if (savedStateBundle?.Data.TryGetValue(BundleRegisterKey, out string restoreRegisterStr) == true &&
            bool.TryParse(restoreRegisterStr, out bool restoreRegister) && restoreRegister)
        {
            resultViewModelManager.RegisterToResult(viewModel);
        }
    }

    public static void SaveRegisterToResult<TResult>(
        this IMvxResultAwaitingViewModel<TResult> viewModel,
        IMvxBundle savedStateBundle,
        IMvxResultViewModelManager resultViewModelManager)
    {
        if (resultViewModelManager.IsRegistered(viewModel) &&
            savedStateBundle?.Data is { } data)
        {
            data[BundleRegisterKey] = true.ToString();
        }
    }

    public static void RegisterToResult<TResult>(
        this IMvxResultAwaitingViewModel<TResult> viewModel,
        IMvxResultViewModelManager resultViewModelManager)
    {
        resultViewModelManager.RegisterToResult(viewModel);
    }

    public static void UnregisterToResult<TResult>(
        this IMvxResultAwaitingViewModel<TResult> viewModel,
        IMvxResultViewModelManager resultViewModelManager)
    {
        resultViewModelManager.UnregisterToResult(viewModel);
    }

    public static void SetResult<TResult>(
        this IMvxResultSettingViewModel<TResult> viewModel,
        TResult result,
        IMvxResultViewModelManager resultViewModelManager)
    {
        resultViewModelManager.SetResult(viewModel, result);
    }
}

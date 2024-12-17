// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public abstract class MvxResultSettingViewModel<TResult> : MvxViewModel, IMvxResultSettingViewModel<TResult>
{
    protected IMvxResultViewModelManager ResultViewModelManager { get; }

    protected MvxResultSettingViewModel(IMvxResultViewModelManager resultViewModelManager)
    {
        ResultViewModelManager = resultViewModelManager;
    }

    public virtual void SetResult(TResult result)
    {
        this.SetResult<TResult>(result, ResultViewModelManager);
    }
}

public abstract class MvxResultSettingViewModel<TParameter, TResult> : MvxResultSettingViewModel<TResult>, IMvxViewModel<TParameter>
{
    protected MvxResultSettingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public abstract void Prepare(TParameter parameter);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public abstract class MvxResultSettingViewModel<TResult> : MvxViewModel, IMvxResultSettingViewModel<TResult>
{
    public virtual void SetResult(TResult result)
    {
        this.SetResult<TResult>(result);
    }
}

public abstract class MvxResultSettingViewModel<TParameter, TResult> : MvxResultSettingViewModel<TResult>, IMvxViewModel<TParameter>
{
    public abstract void Prepare(TParameter parameter);
}

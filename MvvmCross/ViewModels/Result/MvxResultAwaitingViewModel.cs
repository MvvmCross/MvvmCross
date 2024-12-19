// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.ViewModels.Result;

public abstract class MvxResultAwaitingViewModel<TResult>
    : MvxViewModel, IMvxResultAwaitingViewModel<TResult>
{
    protected IMvxResultViewModelManager ResultViewModelManager { get; }

    protected MvxResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
    {
        ResultViewModelManager = resultViewModelManager;
    }

    protected override void ReloadFromBundle(IMvxBundle state)
    {
        base.ReloadFromBundle(state);
        ReloadAndRegisterToResult(state);
    }

    protected override void SaveStateToBundle(IMvxBundle bundle)
    {
        base.SaveStateToBundle(bundle);
        SaveRegisterToResult(bundle);
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        base.ViewDestroy(viewFinishing);

        if (viewFinishing)
        {
            UnregisterToResult();
        }
    }

    public virtual void ReloadAndRegisterToResult(IMvxBundle state)
    {
        this.ReloadAndRegisterToResult(state, ResultViewModelManager);
    }

    public virtual void SaveRegisterToResult(IMvxBundle state)
    {
        this.SaveRegisterToResult<TResult>(state, ResultViewModelManager);
    }

    public virtual void UnregisterToResult()
    {
        this.UnregisterToResult<TResult>(ResultViewModelManager);
    }

    public abstract bool ResultSet(IMvxResultSettingViewModel<TResult> viewModel, TResult result);
}

public abstract class MvxResultAwaitingViewModel<TParameter, TResult>
    : MvxResultAwaitingViewModel<TResult>, IMvxViewModel<TParameter>
{
    protected MvxResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public abstract void Prepare(TParameter parameter);
}

public abstract class MvxResultAwaitingViewModel<TParameter, TResult1, TResult2>
    : MvxMultiResultAwaitingViewModel<TResult1, TResult2>, IMvxViewModel<TParameter>
{
    protected MvxResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public abstract void Prepare(TParameter parameter);
}

public abstract class MvxResultAwaitingViewModel<TParameter, TResult1, TResult2, TResult3>
    : MvxMultiResultAwaitingViewModel<TResult1, TResult2, TResult3>, IMvxViewModel<TParameter>
{
    protected MvxResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public abstract void Prepare(TParameter parameter);
}

public abstract class MvxMultiResultAwaitingViewModel<TResult1, TResult2>
    : MvxResultAwaitingViewModel<TResult1>, IMvxResultAwaitingViewModel<TResult2>
{
    protected MvxMultiResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public override void ReloadAndRegisterToResult(IMvxBundle state)
    {
        base.ReloadAndRegisterToResult(state);
        this.ReloadAndRegisterToResult<TResult2>(state, ResultViewModelManager);
    }

    public override void SaveRegisterToResult(IMvxBundle state)
    {
        base.SaveRegisterToResult(state);
        this.SaveRegisterToResult<TResult2>(state, ResultViewModelManager);
    }

    public override void UnregisterToResult()
    {
        base.UnregisterToResult();
        this.UnregisterToResult<TResult2>(ResultViewModelManager);
    }

    public abstract bool ResultSet(IMvxResultSettingViewModel<TResult2> viewModel, TResult2 result);
}

public abstract class MvxMultiResultAwaitingViewModel<TResult1, TResult2, TResult3>
    : MvxMultiResultAwaitingViewModel<TResult1, TResult2>, IMvxResultAwaitingViewModel<TResult3>
{
    protected MvxMultiResultAwaitingViewModel(IMvxResultViewModelManager resultViewModelManager)
        : base(resultViewModelManager)
    {
    }

    public override void ReloadAndRegisterToResult(IMvxBundle state)
    {
        base.ReloadAndRegisterToResult(state);
        this.ReloadAndRegisterToResult<TResult3>(state, ResultViewModelManager);
    }

    public override void SaveRegisterToResult(IMvxBundle state)
    {
        base.SaveRegisterToResult(state);
        this.SaveRegisterToResult<TResult3>(state, ResultViewModelManager);
    }

    public override void UnregisterToResult()
    {
        base.UnregisterToResult();
        this.UnregisterToResult<TResult3>(ResultViewModelManager);
    }

    public abstract bool ResultSet(IMvxResultSettingViewModel<TResult3> viewModel, TResult3 result);
}

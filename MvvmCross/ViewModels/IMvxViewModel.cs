// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
    public interface IMvxViewModel
    {
        ValueTask ViewCreated();

        ValueTask ViewAppearing();

        ValueTask ViewAppeared();

        ValueTask ViewDisappearing();

        ValueTask ViewDisappeared();

        ValueTask ViewDestroy(bool viewFinishing = true);

        ValueTask Init(IMvxBundle? parameters);

        ValueTask ReloadState(IMvxBundle? state);

        ValueTask Start();

        ValueTask SaveState(IMvxBundle? state);

        ValueTask Prepare();
    }

    public interface IMvxViewModel<TParameter> : IMvxViewModel
    {
        ValueTask Prepare(TParameter parameter);
    }

    //TODO: Can we keep the IMvxViewModel syntax here? Compiler complains
    public interface IMvxViewModelResult<TResult> : IMvxViewModel
    {
        TaskCompletionSource<object>? CloseCompletionSource { get; set; }
    }

    public interface IMvxViewModel<TParameter, TResult> : IMvxViewModel<TParameter>, IMvxViewModelResult<TResult>
    {
    }
}

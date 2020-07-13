// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MvvmCross.ViewModels
{
    public abstract class MvxViewModel
        : MvxNotifyPropertyChanged, IMvxViewModel
    {
        protected MvxViewModel()
        {
        }

        public virtual ValueTask ViewCreated()
        {
            return new ValueTask();
        }

        public virtual ValueTask ViewAppearing()
        {
            return new ValueTask();
        }

        public virtual ValueTask ViewAppeared()
        {
            return new ValueTask();
        }

        public virtual ValueTask ViewDisappearing()
        {
            return new ValueTask();
        }

        public virtual ValueTask ViewDisappeared()
        {
            return new ValueTask();
        }

        public virtual ValueTask ViewDestroy(bool viewFinishing = true)
        {
            return new ValueTask();
        }

        public ValueTask Init(IMvxBundle? parameters)
        {
            return InitFromBundle(parameters);
        }

        public ValueTask ReloadState(IMvxBundle? state)
        {
            return ReloadFromBundle(state);
        }

        public virtual ValueTask Start()
        {
            return new ValueTask();
        }

        public ValueTask SaveState(IMvxBundle? state)
        {
            return SaveStateToBundle(state);
        }

        protected virtual ValueTask InitFromBundle(IMvxBundle? parameters)
        {
            return new ValueTask();
        }

        protected virtual ValueTask ReloadFromBundle(IMvxBundle? state)
        {
            return new ValueTask();
        }

        protected virtual ValueTask SaveStateToBundle(IMvxBundle? bundle)
        {
            return new ValueTask();
        }

        public virtual ValueTask Prepare()
        {
            return new ValueTask();
        }

        public virtual ValueTask Initialize()
        {
            return new ValueTask();
        }
    }

    public abstract class MvxViewModel<TParameter> : MvxViewModel, IMvxViewModel<TParameter>
    {
        public abstract ValueTask Prepare(TParameter parameter);
    }

    //TODO: Not possible to name MvxViewModel, name is MvxViewModelResult for now
    public abstract class MvxViewModelResult<TResult> : MvxViewModel, IMvxViewModelResult<TResult>
    {
        public TaskCompletionSource<object>? CloseCompletionSource { get; set; }

        public override ValueTask ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource?.Task.IsCompleted == false && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            return base.ViewDestroy(viewFinishing);
        }
    }

    public abstract class MvxViewModel<TParameter, TResult> : MvxViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract ValueTask Prepare(TParameter parameter);
    }
}

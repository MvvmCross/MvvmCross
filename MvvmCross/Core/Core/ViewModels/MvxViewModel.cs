// MvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxViewModel
        : MvxNavigatingObject, IMvxViewModel
    {
        protected MvxViewModel()
        {
        }

        public virtual void ViewCreated()
        {
        }

        public virtual void ViewAppearing()
        {
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappearing()
        {
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroy()
        {
        }

        public void Init(IMvxBundle parameters)
        {
            InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            SaveStateToBundle(state);
        }

        protected virtual void InitFromBundle(IMvxBundle parameters)
        {
        }

        protected virtual void ReloadFromBundle(IMvxBundle state)
        {
        }

        protected virtual void SaveStateToBundle(IMvxBundle bundle)
        {
        }

        public virtual void Prepare()
        {
        }

        public virtual Task Initialize()
        {
            return Task.FromResult(true);
        }
    }

    public abstract class MvxViewModel<TParameter> : MvxViewModel, IMvxViewModel<TParameter>
    {
        public async Task Init(string parameter)
        {
            if(!string.IsNullOrEmpty(parameter))
            {
                IMvxJsonConverter serializer;
                if (!Mvx.TryResolve(out serializer))
                {
                    throw new MvxIoCResolveException("There is no implementation of IMvxJsonConverter registered. You need to use the MvvmCross Json plugin or create your own implementation of IMvxJsonConverter.");
                }

                var deserialized = serializer.DeserializeObject<TParameter>(parameter);
                Prepare(deserialized);
                await Initialize();
            }
        }

        public abstract void Prepare(TParameter parameter);
    }

    //TODO: Not possible to name MvxViewModel, name is MvxViewModelResult for now
    public abstract class MvxViewModelResult<TResult> : MvxViewModel, IMvxViewModelResult<TResult>
    {
        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public override void ViewDestroy()
        {
            if (CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();
            base.ViewDestroy();
        }
    }

    public abstract class MvxViewModel<TParameter, TResult> : MvxViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
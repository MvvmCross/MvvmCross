// MvxViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Core.ViewModels
{
    public abstract class MvxViewModel
        : MvxNavigatingObject
          , IMvxViewModel
    {
        protected MvxViewModel()
        {
        }

        public virtual void Appearing()
        {
        }

        public virtual void Appeared()
        {
        }

        public virtual void Disappearing()
        {
        }

        public virtual void Disappeared()
        {
        }

        public void Init(IMvxBundle parameters)
        {
            this.InitFromBundle(parameters);
        }

        public void ReloadState(IMvxBundle state)
        {
            this.ReloadFromBundle(state);
        }

        public virtual void Start()
        {
        }

        public virtual void Destroy()
        {
        }

        public void SaveState(IMvxBundle state)
        {
            this.SaveStateToBundle(state);
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
    }

    public abstract class MvxViewModel<TParameter> : MvxViewModel, IMvxViewModel<TParameter> where TParameter : class
    {
        public async Task Init(string parameter)
        {
            IMvxJsonConverter serializer;
            if (!Mvx.TryResolve(out serializer))
            {
                throw new MvxIoCResolveException("There is no implementation of IMvxJsonConverter registered. You need to use the MvvmCross Json plugin or create your own implementation of IMvxJsonConverter.");
            }

            var deserialized = serializer.DeserializeObject<TParameter>(parameter);
            await Initialize(deserialized);
        }

        public abstract Task Initialize(TParameter parameter);
    }

    //TODO: Not possible to name MvxViewModel, name is MvxViewModelResult for now
    public abstract class MvxViewModelResult<TResult> : MvxViewModel, IMvxViewModelResult<TResult> where TResult : class
    {
        TaskCompletionSource<TResult> _tcs;

        public void SetClose(TaskCompletionSource<TResult> tcs)
        {
            if (tcs == null)
                throw new ArgumentException("TaskCompletionSource should not be null", nameof(tcs));

            _tcs = tcs;
        }

        public virtual Task<bool> Close(TResult result)
        {
            _tcs?.TrySetResult(result);
            return Task.FromResult(Close(this));
        }

        public override void Disappeared()
        {
            _tcs?.TrySetCanceled();
            base.Disappeared();
        }
    }

    public abstract class MvxViewModel<TParameter, TResult> : MvxViewModel, IMvxViewModel<TParameter, TResult> where TParameter : class where TResult : class
    {
        TaskCompletionSource<TResult> _tcs;

        public void SetClose(TaskCompletionSource<TResult> tcs)
        {
            if (tcs == null)
                throw new ArgumentException("TaskCompletionSource should not be null", nameof(tcs));

            _tcs = tcs;
        }

        public abstract Task Initialize(TParameter parameter);
        public virtual Task<bool> Close(TResult result)
        {
            _tcs?.TrySetResult(result);
            return Task.FromResult(Close(this));
        }

        public override void Disappeared()
        {
            _tcs?.TrySetCanceled();
            base.Disappeared();
        }
    }
}
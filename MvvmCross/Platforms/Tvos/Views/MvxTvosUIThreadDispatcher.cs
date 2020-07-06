// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public abstract class MvxTvosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxTvosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
            if (_uiSynchronizationContext == null)
                throw new MvxException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
        }

        public override void ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            }
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                return ExceptionMaskedActionAsync(action, maskExceptions);
            }
            else
            {
                UIApplication.SharedApplication.BeginInvokeOnMainThread(async () =>
                {
                    await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
                });
                return new ValueTask();
            }
        }

        public override bool IsOnMainThread => _uiSynchronizationContext == SynchronizationContext.Current;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public abstract class MvxIosUIThreadDispatcher
        : MvxMainThreadAsyncDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxIosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
            if (_uiSynchronizationContext == null)
                throw new MvxException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");
        }

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                });
            return true;
        }

        public override bool IsOnMainThread => _uiSynchronizationContext == SynchronizationContext.Current;
    }
}

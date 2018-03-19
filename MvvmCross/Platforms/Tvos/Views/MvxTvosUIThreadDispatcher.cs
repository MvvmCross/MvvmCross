// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MvvmCross.Base;
using UIKit;

namespace MvvmCross.Platform.Tvos.Views
{
    public abstract class MvxTvosUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxTvosUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action, bool maskException = true)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                UIApplication.SharedApplication.BeginInvokeOnMainThread(() => 
            {
                if (maskException)
                    ExceptionMaskedAction(action);
                else
                    action();
            });
            return true;
        }
    }
}

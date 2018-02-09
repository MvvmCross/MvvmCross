// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;

using AppKit;
using MvvmCross.Base;

namespace MvvmCross.Platform.Mac.Views
{
    public abstract class MvxMacUIThreadDispatcher
        : MvxMainThreadDispatcher
    {
        private readonly SynchronizationContext _uiSynchronizationContext;

        protected MvxMacUIThreadDispatcher()
        {
            _uiSynchronizationContext = SynchronizationContext.Current;
        }

        public bool RequestMainThreadAction(Action action, 
            bool maskExceptions = true)
        {
            if (_uiSynchronizationContext == SynchronizationContext.Current)
                action();
            else
                NSApplication.SharedApplication.BeginInvokeOnMainThread(() => 
                {
                    if (maskExceptions)
                        ExceptionMaskedAction(action);
                    else
                        action();
                });
            return true;
        }
    }
}

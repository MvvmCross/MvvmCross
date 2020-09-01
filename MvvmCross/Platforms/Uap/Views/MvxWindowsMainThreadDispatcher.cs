// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Core;
using MvvmCross.Base;
using System.Threading;
using static MvvmCross.Base.MvxAsyncPump;

namespace MvvmCross.Platforms.Uap.Views
{
    public class MvxWindowsMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        private readonly CoreDispatcher _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(CoreDispatcher uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
                return true;
            }

            _uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                ExceptionMaskedAction(action, maskExceptions);
            });
            return true;
        }

        public override bool IsOnMainThread
        {
            get
            {
                if (_uiDispatcher.HasThreadAccess)
                    return true;

                if (SynchronizationContext.Current is SingleThreadSynchronizationContext)
                    return true;

                return false;
            }
        }
    }
}

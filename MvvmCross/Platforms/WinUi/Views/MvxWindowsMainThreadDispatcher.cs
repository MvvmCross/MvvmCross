// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Dispatching;
using MvvmCross.Base;
using Windows.UI.Core;

namespace MvvmCross.Platforms.WinUi.Views
{
    public class MvxWindowsMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        private readonly DispatcherQueue _uiDispatcher;

        public MvxWindowsMainThreadDispatcher(DispatcherQueue uiDispatcher)
        {
            _uiDispatcher = uiDispatcher;
        }

        public override bool IsOnMainThread => _uiDispatcher.HasThreadAccess;

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
                return true;
            }

            var queued = _uiDispatcher.TryEnqueue(DispatcherQueuePriority.Normal,
                () => ExceptionMaskedAction(action, maskExceptions));
            return queued;
        }
    }
}

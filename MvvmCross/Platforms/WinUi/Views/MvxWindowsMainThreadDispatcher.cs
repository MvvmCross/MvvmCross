// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Core;
using MvvmCross.Base;
using Microsoft.UI.Dispatching;

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

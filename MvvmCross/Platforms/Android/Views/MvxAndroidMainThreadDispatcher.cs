// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using MvvmCross.Base;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public override ValueTask ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                ExceptionMaskedAction(action, maskExceptions);
            }
            else
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }

            return new ValueTask();
        }

        public override ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
            {
                return ExceptionMaskedActionAsync(action, maskExceptions);
            }
            else
            {
                Application.SynchronizationContext.Post(async _ =>
                {
                    await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(true);
                }, null);

                return new ValueTask();
            }
        }
    }
}

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
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
                action();
            else
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }

            return true;
        }
    }
}

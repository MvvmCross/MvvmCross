// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.App;
using MvvmCross.Base;
using static MvvmCross.Base.MvxAsyncPump;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (IsOnMainThread)
                ExceptionMaskedAction(action, maskExceptions);
            else
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    ExceptionMaskedAction(action, maskExceptions);
                }, null);
            }

            return true;
        }

        public override bool IsOnMainThread
        {
            get
            {
                if (Application.SynchronizationContext == SynchronizationContext.Current)
                    return true;

                if (SynchronizationContext.Current is SingleThreadSynchronizationContext)
                    return true;

                return false;
            }
        }
    }
}

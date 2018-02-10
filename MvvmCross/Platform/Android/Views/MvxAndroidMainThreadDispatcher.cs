// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.App;
using MvvmCross.Base;

namespace MvvmCross.Platform.Android.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadDispatcher
    {
        public bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            if (Application.SynchronizationContext == SynchronizationContext.Current)
                action();
            else
            {
                Application.SynchronizationContext.Post(ignored => 
                {
                    if (maskExceptions)
                        ExceptionMaskedAction(action);
                    else
                        action();
                }, null);
            }

            return true;
        }
    }
}

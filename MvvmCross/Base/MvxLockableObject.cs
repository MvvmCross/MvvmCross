// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Base
{
    public abstract class MvxLockableObject
    {
        private readonly object _lockObject = new object();

        protected void RunSyncWithLock(Action action)
        {
            MvxLockableObjectHelpers.RunSyncWithLock(_lockObject, action);
        }

        protected void RunAsyncWithLock(Action action)
        {
            MvxLockableObjectHelpers.RunAsyncWithLock(_lockObject, action);
        }

        protected void RunSyncOrAsyncWithLock(Action action, Action whenComplete = null)
        {
            MvxLockableObjectHelpers.RunSyncOrAsyncWithLock(_lockObject, action, whenComplete);
        }
    }
}

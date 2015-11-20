// MvxLockableObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Core
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
// MvxAllThreadDispatchingObject.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;

    public abstract class MvxAllThreadDispatchingObject
        : MvxMainThreadDispatchingObject
    {
        private readonly object _lockObject = new object();

        protected void RunSyncWithLock(Action action)
        {
            MvxLockableObjectHelpers.RunSyncWithLock(this._lockObject, action);
        }

        protected void RunAsyncWithLock(Action action)
        {
            MvxLockableObjectHelpers.RunAsyncWithLock(this._lockObject, action);
        }

        protected void RunSyncOrAsyncWithLock(Action action, Action whenComplete = null)
        {
            MvxLockableObjectHelpers.RunSyncOrAsyncWithLock(this._lockObject, action, whenComplete);
        }
    }
}
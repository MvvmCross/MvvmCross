// MvxLockableObjectHelpers.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;

namespace Cirrious.CrossCore.Core
{
    public static class MvxLockableObjectHelpers
    {
        public static void RunSyncWithLock(object lockObject, Action action)
        {
            lock (lockObject)
            {
                action();
            }
        }

        public static void RunAsyncWithLock(object lockObject, Action action)
        {
            MvxAsyncDispatcher.BeginAsync(() =>
                {
                    lock (lockObject)
                    {
                        action();
                    }
                });
        }

        public static void RunSyncOrAsyncWithLock(object lockObject, Action action, Action whenComplete = null)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    action();
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }

                whenComplete?.Invoke();
            }
            else
            {
                MvxAsyncDispatcher.BeginAsync(() =>
                    {
                        lock (lockObject)
                        {
                            action();
                        }

                        whenComplete?.Invoke();
                    });
            }
        }
    }
}
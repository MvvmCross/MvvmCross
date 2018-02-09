// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Base
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
            Task.Run(() =>
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
                Task.Run(() =>
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadAsyncDispatcher AsyncDispatcher => MvxMainThreadDispatcher.Instance as IMvxMainThreadAsyncDispatcher;

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
            InvokeOnMainThreadAsync(action, maskExceptions);
        }

        protected Task InvokeOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            // this corner case should only happen when there is no IoC
            // i.e. when running in a UnitTest environment, falling back
            // to just executing action
            if (AsyncDispatcher == null)
            {
                try
                {
                    action();
                }
                catch
                {
                    if (!maskExceptions)
                        throw;
                }

                return Task.CompletedTask;
            }

            return AsyncDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}

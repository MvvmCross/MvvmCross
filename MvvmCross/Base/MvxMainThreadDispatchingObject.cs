// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher AsyncDispatcher => MvxMainThreadDispatcher.Instance as IMvxMainThreadDispatcher;

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
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
            }

            AsyncDispatcher.ExecuteOnMainThread(action, maskExceptions);
        }

        protected ValueTask InvokeOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true)
        {
            if (AsyncDispatcher == null)
            {
                try
                {
                    return action();
                }
                catch
                {
                    if (!maskExceptions)
                        throw;
                }
            }

            return AsyncDispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}

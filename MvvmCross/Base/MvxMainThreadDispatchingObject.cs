// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Hosting;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadAsyncDispatcher? AsyncDispatcher =>
            MvxHost.Current?.Services.GetService<IMvxMainThreadAsyncDispatcher>();

        protected void InvokeOnMainThread(Action action, bool maskExceptions = true)
        {
            InvokeOnMainThreadAsync(action, maskExceptions);
        }

        protected Task InvokeOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            var dispatcher = AsyncDispatcher;

            // Fall back to direct execution when no host is running (e.g. unit tests).
            if (dispatcher == null)
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

            return dispatcher.ExecuteOnMainThreadAsync(action, maskExceptions);
        }
    }
}

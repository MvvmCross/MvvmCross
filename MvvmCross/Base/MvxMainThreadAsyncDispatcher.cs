﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadAsyncDispatcher : MvxMainThreadDispatcher, IMvxMainThreadAsyncDispatcher
    {
        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            var asyncAction = new Func<Task>(() =>
            {
                action();
                return Task.CompletedTask;
            });
            return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            var completion = new TaskCompletionSource<bool>();
            var syncAction = new Action(async () =>
            {
                await action();
                completion.SetResult(true);
            });
            RequestMainThreadAction(syncAction, maskExceptions);

            // If we're already on main thread, then the action will
            // have already completed at this point, so can just return
            if (completion.Task.IsCompleted)
                return;

            // Make sure we don't introduce weird locking issues  
            // blocking on the completion source by jumping onto
            // a new thread to wait
            await Task.Run(async () => await completion.Task);
        }
    }
}

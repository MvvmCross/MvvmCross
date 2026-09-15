// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadAsyncDispatcher : MvxMainThreadDispatcher, IMvxMainThreadAsyncDispatcher
    {
        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            if (action == null)
                return Task.CompletedTask;

            var asyncAction = new Func<Task>(() =>
            {
                action();
                return Task.CompletedTask;
            });
            return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            if (action == null)
                return;

            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var syncAction = new Action(async () =>
            {
                try
                {
                    await action();
                    completion.SetResult(true);
                }
                catch (Exception exception)
                {
                    // Fault the completion source instead of letting the exception
                    // escape the async void lambda, where it would be rethrown on the
                    // SynchronizationContext and crash the process
                    completion.SetException(exception);
                }
            });
            RequestMainThreadAction(syncAction, maskExceptions);

            try
            {
                // The completion source runs continuations asynchronously,
                // so awaiting directly cannot block the main thread that
                // completes it; when the action already ran inline the task
                // is completed and this returns synchronously
                await completion.Task;
            }
            catch (Exception exception)
            {
                MvxLogHost.Default?.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    MvxLogHost.Default?.LogWarning(exception, "Exception masked");
                else
                    throw;
            }
        }

        public abstract override bool IsOnMainThread { get; }
    }
}

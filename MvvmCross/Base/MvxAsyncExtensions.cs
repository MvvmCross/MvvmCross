// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    /// <summary>
    /// Provides a pump that supports running asynchronous methods on the current thread.
    /// 
    /// This code is based on Stephen Toub's AsyncPump:
    /// https://blogs.msdn.microsoft.com/pfxteam/2012/02/02/await-synchronizationcontext-and-console-apps-part-3/
    /// </summary>
    internal static class MvxAsyncPump
    {
        /// <summary>
        /// Runs the specified asynchronous method.
        /// </summary>
        /// <param name="asyncMethod">The asynchronous method to execute.</param>
        public static void Run(Func<Task> asyncMethod)
        {
            if (asyncMethod == null)
                throw new ArgumentNullException(nameof(asyncMethod));

            var oldContext = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var synchronousContext = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(synchronousContext);

                // Invoke the function and alert the context to when it completes
                var task = asyncMethod();
                if (task == null)
                    throw new InvalidOperationException("No task provided.");

                task.ContinueWith(delegate { synchronousContext.Complete(); }, TaskScheduler.Default);

                // Pump continuations and propagate any exceptions
                synchronousContext.RunOnCurrentThread();

                task.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(oldContext); }
        }

        /// <summary>
        /// Provides a SynchronizationContext that is single-threaded.
        /// </summary>
        private sealed class SingleThreadSynchronizationContext : SynchronizationContext
        {
            private readonly BlockingCollection<Tuple<SendOrPostCallback, object>> _queue =
                new BlockingCollection<Tuple<SendOrPostCallback, object>>();

            private readonly Thread _thread = Thread.CurrentThread;

            internal SingleThreadSynchronizationContext()
            {
            }

            /// <summary>
            /// Dispatches an asynchronous message to the synchronization context.
            /// </summary>
            /// <param name="callback">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback callback, object state)
            {
                if (callback == null)
                    throw new ArgumentNullException("d");

                _queue.Add(Tuple.Create(callback, state));
            }

            public override void Send(SendOrPostCallback callback, object state) 
                => throw new NotSupportedException("Synchronously sending is not supported.");

            /// <summary>
            /// Runs an loop to process all queued work items.
            /// </summary>
            public void RunOnCurrentThread()
            {
                foreach (var workItem in _queue.GetConsumingEnumerable())
                    workItem.Item1(workItem.Item2);
            }

            /// <summary>
            /// Notifies the context that no more work will arrive.
            /// </summary>
            public void Complete() => _queue.CompleteAdding();
        }
    }
}

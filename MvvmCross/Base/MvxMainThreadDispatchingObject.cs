// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher Dispatcher => MvxMainThreadDispatcher.Instance;
        protected IMvxMainThreadAsyncDispatcher AsyncDispatcher => MvxMainThreadDispatcher.Instance as IMvxMainThreadAsyncDispatcher;

        protected void InvokeOnMainThread(Action action)
        {
            Dispatcher?.RequestMainThreadAction(action);
        }

        protected Task InvokeOnMainThreadAsync(Action action)
        {
            return AsyncDispatcher?.ExecuteOnMainThreadAsync(action);
        }
    }
}

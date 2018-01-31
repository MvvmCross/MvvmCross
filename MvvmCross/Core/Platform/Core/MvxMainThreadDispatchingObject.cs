// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Platform.Core
{
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher Dispatcher => MvxMainThreadDispatcher.Instance;

        protected void InvokeOnMainThread(Action action)
        {
            Dispatcher?.RequestMainThreadAction(action);
        }
    }
}
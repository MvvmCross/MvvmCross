// MvxMainThreadDispatchingObject.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;

    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher Dispatcher => MvxMainThreadDispatcher.Instance;

        protected void InvokeOnMainThread(Action action)
        {
            this.Dispatcher?.RequestMainThreadAction(action);
        }
    }
}
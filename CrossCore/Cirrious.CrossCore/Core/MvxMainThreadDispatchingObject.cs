// MvxMainThreadDispatchingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Interfaces.ServiceProvider;

namespace Cirrious.CrossCore.Core
{
    public abstract class MvxMainThreadDispatchingObject
        : IMvxServiceConsumer
    {
        protected IMvxMainThreadDispatcher Dispatcher
        {
			get { return this.GetService<IMvxMainThreadDispatcherProvider>().Dispatcher; }
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (Dispatcher != null)
                Dispatcher.RequestMainThreadAction(action);
        }
    }
}
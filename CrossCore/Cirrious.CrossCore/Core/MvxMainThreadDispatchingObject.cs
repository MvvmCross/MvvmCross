// MvxMainThreadDispatchingObject.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Runtime.Serialization;

namespace Cirrious.CrossCore.Core
{
    [DataContract]
    public abstract class MvxMainThreadDispatchingObject
    {
        protected IMvxMainThreadDispatcher Dispatcher
        {
            get { return MvxMainThreadDispatcher.Instance; }
        }

        protected void InvokeOnMainThread(Action action)
        {
            if (Dispatcher != null)
                Dispatcher.RequestMainThreadAction(action);
        }
    }
}
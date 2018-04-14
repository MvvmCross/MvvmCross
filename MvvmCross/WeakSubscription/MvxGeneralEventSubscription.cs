// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;

namespace MvvmCross.WeakSubscription
{
    public class MvxGeneralEventSubscription
        : MvxWeakEventSubscription<object, EventArgs>
    {
        public MvxGeneralEventSubscription(object source,
                                           EventInfo eventInfo,
                                           EventHandler<EventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler(OnSourceEvent);
        }
    }

    /*
    public class MvxGeneralEventSubscription<TSource, TEventArgs>
        : MvxWeakEventSubscription<TSource, TEventArgs>
        where TSource : class
        where TEventArgs : EventArgs
    {
        public MvxGeneralEventSubscription(TSource source,
                                           EventInfo eventInfo,
                                           EventHandler<TEventArgs> eventHandler)
            : base(source, eventInfo, eventHandler)
        {
        }

        public MvxGeneralEventSubscription(TSource source,
                                           string eventName,
                                           EventHandler<TEventArgs> eventHandler)
            : base(source, typeof(TSource).GetEvent(eventName), eventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new EventHandler<TEventArgs>(OnSourceEvent);
        }
    }
     */
}

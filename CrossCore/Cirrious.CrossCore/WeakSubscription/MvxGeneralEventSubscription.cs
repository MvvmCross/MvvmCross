// MvxGeneralEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
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
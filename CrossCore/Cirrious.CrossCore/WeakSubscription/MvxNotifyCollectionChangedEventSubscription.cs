// MvxNotifyCollectionChangedEventSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Cirrious.CrossCore.WeakSubscription
{
    public class MvxNotifyCollectionChangedEventSubscription
        : MvxWeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        private static readonly EventInfo EventInfo = typeof (INotifyCollectionChanged).GetEvent("CollectionChanged");

        public MvxNotifyCollectionChangedEventSubscription(INotifyCollectionChanged source,
                                                           EventHandler<NotifyCollectionChangedEventArgs>
                                                               targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
}
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
        private static readonly EventInfo EventInfo = typeof(INotifyCollectionChanged).GetEvent("CollectionChanged");

        // This code ensures the CollectionChanged event is not stripped by Xamarin linker
        // see https://github.com/MvvmCross/MvvmCross/pull/453
        public static void LinkerPleaseInclude(INotifyCollectionChanged iNotifyCollectionChanged)
        {
            iNotifyCollectionChanged.CollectionChanged += (sender, e) => { };
        }

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
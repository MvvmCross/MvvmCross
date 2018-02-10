// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.Reflection;

namespace MvvmCross.WeakSubscription
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
                                                           EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
}

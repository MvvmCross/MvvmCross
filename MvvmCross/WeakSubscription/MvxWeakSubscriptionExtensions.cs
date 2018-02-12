// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using MvvmCross.Base;

namespace MvvmCross.WeakSubscription
{
    public static class MvxWeakSubscriptionExtensions
    {
        public static MvxNotifyPropertyChangedEventSubscription WeakSubscribe(this INotifyPropertyChanged source,
                                                                              EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new MvxNotifyPropertyChangedEventSubscription(source, eventHandler);
        }

        public static MvxNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               Expression<Func<T>> property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new MvxNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static MvxNamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               string property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new MvxNamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static MvxNotifyCollectionChangedEventSubscription WeakSubscribe(this INotifyCollectionChanged source,
                                                                                EventHandler<NotifyCollectionChangedEventArgs> eventHandler)
        {
            return new MvxNotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        public static MvxGeneralEventSubscription WeakSubscribe(this EventInfo eventInfo,
                                                                object source,
                                                                EventHandler<EventArgs> eventHandler)
        {
            return new MvxGeneralEventSubscription(source, eventInfo, eventHandler);
        }

        public static MvxValueEventSubscription<T> WeakSubscribe<T>(this EventInfo eventInfo,
                                                                    object source,
                                                                    EventHandler<MvxValueEventArgs<T>> eventHandler)
        {
            return new MvxValueEventSubscription<T>(source, eventInfo, eventHandler);
        }

        public static MvxCanExecuteChangedEventSubscription WeakSubscribe(this ICommand source,
                                                                          EventHandler<EventArgs> eventHandler)
        {
            return new MvxCanExecuteChangedEventSubscription(source, eventHandler);
        }

        public static MvxWeakEventSubscription<TSource> WeakSubscribe<TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new MvxWeakEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static MvxWeakEventSubscription<TSource, TEventArgs> WeakSubscribe<TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new MvxWeakEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}

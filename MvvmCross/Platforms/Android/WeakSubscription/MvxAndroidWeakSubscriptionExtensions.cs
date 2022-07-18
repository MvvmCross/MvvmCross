// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Platforms.Android.WeakSubscription
{
    public static class MvxAndroidWeakSubscriptionExtensions
    {
        public static MvxJavaEventSubscription<TSource> WeakSubscribe<TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new MvxJavaEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static MvxAndroidTargetEventSubscription<TSource, TEventArgs> WeakSubscribe<TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new MvxAndroidTargetEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}

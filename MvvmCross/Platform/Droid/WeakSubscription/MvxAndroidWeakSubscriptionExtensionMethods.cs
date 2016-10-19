using System;

namespace MvvmCross.Platform.Droid.WeakSubscription
{
    public static class MvxAndroidWeakSubscriptionExtensionMethods
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
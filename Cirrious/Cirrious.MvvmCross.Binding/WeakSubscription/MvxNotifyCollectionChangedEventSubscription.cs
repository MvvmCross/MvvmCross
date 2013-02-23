using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.WeakSubscription
{
    public class MvxNotifyCollectionChangedEventSubscription
        : MvxWeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        private static readonly EventInfo EventInfo = typeof(INotifyCollectionChanged).GetEvent("CollectionChanged");

        public MvxNotifyCollectionChangedEventSubscription(INotifyCollectionChanged source, EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
        {
        }

        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
}
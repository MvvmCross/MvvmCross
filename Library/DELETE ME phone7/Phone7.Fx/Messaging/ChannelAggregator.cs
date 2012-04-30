using System;
using System.Collections.Generic;
using System.Linq;

namespace Phone7.Fx.Messaging
{
    public  class ChannelAggregator
    {
       
        private static IDispatcherFacade uiDispatcher;
        private static readonly List<object> _subscriptions = new List<object>();

      
        private static IDispatcherFacade UIDispatcher
        {
            get { return uiDispatcher ?? (uiDispatcher = new DefaultDispatcher()); }
        }


        public static ChannelSubscriptionToken Subscribe<TPayload>(Action<TPayload> action)
        {
            return Subscribe(action, ThreadOption.PublisherThread);
        }


        public static ChannelSubscriptionToken Subscribe<TPayload>(Action<TPayload> action, ThreadOption threadOption)
        {
            return Subscribe(action, threadOption, false);
        }


        public static ChannelSubscriptionToken Subscribe<TPayload>(Action<TPayload> action, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, ThreadOption.PublisherThread, keepSubscriberReferenceAlive);
        }


        public static ChannelSubscriptionToken Subscribe<TPayload>(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive)
        {
            return Subscribe(action, threadOption, keepSubscriberReferenceAlive, null);
        }

        public static ChannelSubscriptionToken Subscribe<TPayload>(Action<TPayload> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TPayload> filter)
        {
            DelegateReference actionReference = new DelegateReference(action, keepSubscriberReferenceAlive);
            DelegateReference filterReference;
            filterReference = filter != null ? new DelegateReference(filter, keepSubscriberReferenceAlive) : new DelegateReference(new Predicate<TPayload>(obj=> true), true);

            ChannelSubscription<TPayload> subscription;
            switch (threadOption)
            {
                case ThreadOption.PublisherThread:
                    subscription = new ChannelSubscription<TPayload>(actionReference, filterReference);
                    break;
                case ThreadOption.BackgroundThread:
                //    subscription = new BackgroundEventSubscription<TPayload>(actionReference, filterReference);
                    throw new NotImplementedException();
                case ThreadOption.UIThread:
                    subscription = new DispatcherChannelSubscription<TPayload>(actionReference, filterReference, UIDispatcher);
                    break;
                default:
                    subscription = new ChannelSubscription<TPayload>(actionReference, filterReference);
                    break;
            }


            //return base.InternalSubscribe(subscription);
            subscription.SubscriptionToken = new ChannelSubscriptionToken();
            lock (_subscriptions)
            {
                _subscriptions.Add(subscription);
            }
            return subscription.SubscriptionToken;
        }


        public static void Publish<TPayload>(TPayload payload)
        {
            List<Action<object[]>> executionStrategies = new List<Action<object[]>>();

            lock (_subscriptions)
            {
                for (var i = _subscriptions.Count - 1; i >= 0; i--)
                {
                    var elem = _subscriptions[i] as ChannelSubscription<TPayload>;
                    if (elem == null)
                        continue;
                    Action<object[]> listItem =
                        ((ChannelSubscription<TPayload>)_subscriptions[i]).GetExecutionStrategy();

                    if (listItem == null)
                    {
                        // Prune from main list. Log?
                        _subscriptions.RemoveAt(i);
                    }
                    else
                    {
                        executionStrategies.Add(listItem);
                    }
                }
            }
            //base.InternalPublish(payload);

            foreach (var executionStrategy in executionStrategies)
            {
                executionStrategy(new object[] { payload });
            }
        }


        public static void Unsubscribe<TPayload>(Action<TPayload> subscriber)
        {
            lock (_subscriptions)
            {
                ChannelSubscription<TPayload> eventSubscription = _subscriptions.Cast<ChannelSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
                if (eventSubscription != null)
                {
                    _subscriptions.Remove(eventSubscription);
                }
            }
        }

        public static bool Contains<TPayload>(Action<TPayload> subscriber)
        {
            ChannelSubscription<TPayload> eventSubscription;
            lock (_subscriptions)
            {
                eventSubscription = _subscriptions.Cast<ChannelSubscription<TPayload>>().FirstOrDefault(evt => evt.Action == subscriber);
            }
            return eventSubscription != null;
        }
    }
}
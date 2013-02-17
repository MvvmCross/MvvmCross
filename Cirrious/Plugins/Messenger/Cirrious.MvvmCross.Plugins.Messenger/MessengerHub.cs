﻿// MessengerHub.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#warning TODO - acknowledge the XPlatUtils parentage!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Plugins.Messenger.Subscriptions;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class MessengerHub : IMessenger
    {
        private readonly Dictionary<Type, Dictionary<Guid, BaseSubscription>> _subscriptions =
            new Dictionary<Type, Dictionary<Guid, BaseSubscription>>();

		public SubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrong = false)
            where TMessage : BaseMessage
        {
            if (deliveryAction == null)
            {
                throw new ArgumentNullException("deliveryAction");
            }

            BaseSubscription subscription;

            if (useStrong)
                subscription = new StrongSubscription<TMessage>(deliveryAction);
            else
                subscription = new WeakSubscription<TMessage>(deliveryAction);

            lock (this)
            {
                Dictionary<Guid, BaseSubscription> messageSubscriptions;
                if (!_subscriptions.TryGetValue(typeof (TMessage), out messageSubscriptions))
                {
                    messageSubscriptions = new Dictionary<Guid, BaseSubscription>();
                    _subscriptions[typeof (TMessage)] = messageSubscriptions;
                }
				MvxTrace.Trace("Adding subscription {0} for {1}", subscription.Id, typeof(TMessage).Name);
                messageSubscriptions[subscription.Id] = subscription;
            }

            return new SubscriptionToken(subscription.Id, deliveryAction);
        }

		public void Unsubscribe<TMessage>(SubscriptionToken subscriptionId) where TMessage : BaseMessage
        {
            lock (this)
            {
                Dictionary<Guid, BaseSubscription> messageSubscriptions;
                if (_subscriptions.TryGetValue(typeof (TMessage), out messageSubscriptions))
                {
                    if (messageSubscriptions.ContainsKey(subscriptionId.Id))
                    {
						MvxTrace.Trace("Removing subscription {0}", subscriptionId);
                        messageSubscriptions.Remove(subscriptionId.Id);
                        // Note - we could also remove messageSubscriptions if empty here
                        //      - but this isn't needed in our typical apps
                    }
                }
            }
        }

        public void Publish<TMessage> (TMessage message) where TMessage : BaseMessage
		{
			if (message == null) {
				throw new ArgumentNullException ("message");
			}

            List<TypedSubscription<TMessage>> toNotify = null;
            lock (this)
            {
				MvxTrace.Trace("Found {0} subscriptions of all types", _subscriptions.Count);
				foreach (var t in _subscriptions.Keys)
				{
					MvxTrace.Trace("Found  subscriptions for {0}", t.Name);
				}
				Dictionary<Guid, BaseSubscription> messageSubscriptions;
                if (_subscriptions.TryGetValue(typeof (TMessage), out messageSubscriptions))
                {
					MvxTrace.Trace("Found {0} messages of type {1}", messageSubscriptions.Values.Count, typeof(TMessage).Name);
                    toNotify = messageSubscriptions.Values.Select(x => x as TypedSubscription<TMessage>).ToList();
                }
            }

            if (toNotify == null)
            {
                MvxTrace.Trace("Nothing registered for messages of type {0}", message.GetType().Name);
                return;
            }

            var allSucceeded = true;
            foreach (var subscription in toNotify)
            {
                allSucceeded &= subscription.TypedInvoke(message);
            }

            if (!allSucceeded)
            {
                MvxTrace.Trace("One or more listeners failed - purge scheduled");
                SchedulePurge<TMessage>();
            }
        }

        private readonly Dictionary<Type, bool> _scheduledPurges = new Dictionary<Type, bool>();

        private void SchedulePurge<TMessage>()
            where TMessage : BaseMessage
        {
            lock (this)
            {
                _scheduledPurges[typeof (TMessage)] = true;
                if (_scheduledPurges.Count == 1)
                {
                    ThreadPool.QueueUserWorkItem(ignored => DoPurge());
                }
            }
        }

        private void DoPurge()
        {
            List<Type> toPurge = null;
            lock (this)
            {
                toPurge = _scheduledPurges.Select(x => x.Key).ToList();
                _scheduledPurges.Clear();
            }

            foreach (var type in toPurge)
            {
                PurgeMessagesOfType(type);
            }
        }

        private void PurgeMessagesOfType(Type type)
        {
            lock (this)
            {
                Dictionary<Guid, BaseSubscription> messageSubscriptions;
                if (!_subscriptions.TryGetValue(type, out messageSubscriptions))
                {
                    return;
                }

                var deadSubscriptionIds = new List<Guid>();
                foreach (var subscription in messageSubscriptions)
                {
                    if (!subscription.Value.IsAlive)
                    {
                        deadSubscriptionIds.Add(subscription.Key);
                    }
                }

				MvxTrace.Trace("Purging {0} subscriptions", deadSubscriptionIds.Count);
				foreach (var id in deadSubscriptionIds)
                {
                    messageSubscriptions.Remove(id);
                }
            }
        }
    }
}
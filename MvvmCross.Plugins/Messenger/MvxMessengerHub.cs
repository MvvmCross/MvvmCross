// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Plugin.Messenger.Subscriptions;
using MvvmCross.Plugin.Messenger.ThreadRunners;

namespace MvvmCross.Plugin.Messenger
{
#nullable enable
    // Note - the original inspiration for this code was XPlatUtils from JonathonPeppers
    // - https://github.com/jonathanpeppers/XPlatUtils
    // - inspiration consumed, ripped apart and loved under Ms-PL
    [Preserve(AllMembers = true)]
    public class MvxMessengerHub : IMvxMessenger
    {
        private readonly Dictionary<Type, MvxMessage> _messageDictionary = new Dictionary<Type, MvxMessage>();

        private readonly object _locker = new object();

        private readonly Dictionary<Type, Dictionary<Guid, BaseSubscription>> _subscriptions =
            new Dictionary<Type, Dictionary<Guid, BaseSubscription>>();

        private readonly Dictionary<Type, bool> _scheduledPurges = new Dictionary<Type, bool>();

        public MvxSubscriptionToken Subscribe<TMessage>(
            Action<TMessage> deliveryAction,
            MvxReference reference = MvxReference.Weak,
            string? tag = null,
            bool isSticky = false)
            where TMessage : MvxMessage
        {
            return SubscribeInternal(deliveryAction, new MvxSimpleActionRunner(), reference, tag, isSticky);
        }

        public MvxSubscriptionToken SubscribeOnMainThread<TMessage>(
            Action<TMessage> deliveryAction,
            MvxReference reference = MvxReference.Weak,
            string? tag = null,
            bool isSticky = false)
            where TMessage : MvxMessage
        {
            return SubscribeInternal(deliveryAction, new MvxMainThreadActionRunner(), reference, tag, isSticky);
        }

        public MvxSubscriptionToken SubscribeOnThreadPoolThread<TMessage>(
            Action<TMessage> deliveryAction,
            MvxReference reference = MvxReference.Weak,
            string? tag = null,
            bool isSticky = false)
            where TMessage : MvxMessage
        {
            return SubscribeInternal(deliveryAction, new MvxThreadPoolActionRunner(), reference, tag, isSticky);
        }

        private MvxSubscriptionToken SubscribeInternal<TMessage>(
            Action<TMessage> deliveryAction,
            IMvxActionRunner actionRunner,
            MvxReference reference,
            string? tag,
            bool isSticky) where TMessage : MvxMessage
        {
            if (deliveryAction == null)
            {
                throw new ArgumentNullException(nameof(deliveryAction));
            }

            BaseSubscription subscription;

            switch (reference)
            {
                case MvxReference.Strong:
                    subscription = new StrongSubscription<TMessage>(actionRunner, deliveryAction, tag);
                    break;

                case MvxReference.Weak:
                    subscription = new WeakSubscription<TMessage>(actionRunner, deliveryAction, tag);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(reference), "reference type unexpected " + reference);
            }

            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    messageSubscriptions = new Dictionary<Guid, BaseSubscription>();
                    _subscriptions[typeof(TMessage)] = messageSubscriptions;
                }
                MvxPluginLog.Instance?.Log(LogLevel.Trace, "Adding subscription {0} for {1}", subscription.Id, typeof(TMessage).Name);
                messageSubscriptions[subscription.Id] = subscription;

                PublishSubscriberChangeMessage<TMessage>(messageSubscriptions);
            }

            if (isSticky && _messageDictionary.ContainsKey(typeof(TMessage)))
            {
                deliveryAction.Invoke((TMessage)_messageDictionary[typeof(TMessage)]);
            }

            return new MvxSubscriptionToken(
                subscription.Id,
                () => InternalUnsubscribe<TMessage>(subscription.Id),
                deliveryAction);
        }

        public void Unsubscribe<TMessage>(MvxSubscriptionToken mvxSubscriptionId) where TMessage : MvxMessage
        {
            InternalUnsubscribe<TMessage>(mvxSubscriptionId.Id);
        }

        private void InternalUnsubscribe<TMessage>(Guid subscriptionGuid) where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions) &&
                    messageSubscriptions.ContainsKey(subscriptionGuid))
                {
                    MvxPluginLog.Instance?.Log(LogLevel.Trace, "Removing subscription {0}", subscriptionGuid);
                    messageSubscriptions.Remove(subscriptionGuid);
                    // Note - we could also remove messageSubscriptions if empty here
                    //      - but this isn't needed in our typical apps
                }

                PublishSubscriberChangeMessage<TMessage>(messageSubscriptions);
            }
        }

        protected virtual void PublishSubscriberChangeMessage<TMessage>(
            Dictionary<Guid, BaseSubscription> messageSubscriptions)
            where TMessage : MvxMessage
        {
            PublishSubscriberChangeMessage(typeof(TMessage), messageSubscriptions);
        }

        protected virtual void PublishSubscriberChangeMessage(
            Type messageType,
            Dictionary<Guid, BaseSubscription> messageSubscriptions)
        {
            var newCount = messageSubscriptions?.Count ?? 0;
            Publish(new MvxSubscriberChangeMessage(this, messageType, newCount));
        }

        public bool HasSubscriptionsFor<TMessage>()
            where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    return false;
                }
                return messageSubscriptions.Any();
            }
        }

        public int CountSubscriptionsFor<TMessage>() where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    return 0;
                }
                return messageSubscriptions.Count;
            }
        }

        public bool HasSubscriptionsForTag<TMessage>(string tag) where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    return false;
                }
                return messageSubscriptions.Any(x => x.Value.Tag == tag);
            }
        }

        public int CountSubscriptionsForTag<TMessage>(string tag) where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    return 0;
                }
                return messageSubscriptions.Count(x => x.Value.Tag == tag);
            }
        }

        public IList<string> GetSubscriptionTagsFor<TMessage>() where TMessage : MvxMessage
        {
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    typeof(TMessage), out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    return new List<string>(0);
                }
                return messageSubscriptions.Select(x => x.Value.Tag).ToList();
            }
        }

        public void Publish<TMessage>(TMessage message, bool isSticky = false) where TMessage : MvxMessage
        {
            if (typeof(TMessage) == typeof(MvxMessage))
            {
                MvxPluginLog.Instance?.Log(LogLevel.Warning,
                               "MvxMessage publishing not allowed - this normally suggests non-specific generic used in calling code - switching to message.GetType()");
                Publish(message, message.GetType(), isSticky);
                return;
            }
            Publish(message, typeof(TMessage), isSticky);
        }

        public void Publish(MvxMessage message, bool isSticky = false)
        {
            Publish(message, message.GetType(), isSticky);
        }

        public void Publish(MvxMessage message, Type messageType, bool isSticky = false)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            List<BaseSubscription>? toNotify = null;
            lock (_locker)
            {
                if (_subscriptions.TryGetValue(messageType, out Dictionary<Guid, BaseSubscription> messageSubscriptions))
                {
                    toNotify = messageSubscriptions.Values.ToList();
                }
            }

            if (isSticky)
            {
                _messageDictionary[message.GetType()] = message;
            }

            if (toNotify == null || toNotify.Count == 0)
            {
                MvxPluginLog.Instance?.Log(LogLevel.Trace, "Nothing registered for messages of type {0}", messageType.Name);
                return;
            }

            var allSucceeded = true;
            foreach (var subscription in toNotify)
            {
                allSucceeded &= subscription.Invoke(message);
            }

            if (!allSucceeded)
            {
                MvxPluginLog.Instance?.Log(LogLevel.Trace, "One or more listeners failed - purge scheduled");
                SchedulePurge(messageType);
            }
        }

        public void RequestPurge(Type messageType)
        {
            SchedulePurge(messageType);
        }

        public void RequestPurgeAll()
        {
            lock (_locker)
            {
                SchedulePurge(_subscriptions.Keys.ToArray());
            }
        }

        public void RemoveSticky<TMessageType>() where TMessageType : MvxMessage
        {
            var key = typeof(TMessageType);
            if (_messageDictionary.ContainsKey(key))
            {
                _messageDictionary.Remove(key);
            }
        }

        private void SchedulePurge(params Type[] messageTypes)
        {
            lock (_locker)
            {
                var threadPoolTaskAlreadyRequested = _scheduledPurges.Count > 0;
                foreach (var messageType in messageTypes)
                    _scheduledPurges[messageType] = true;

                if (!threadPoolTaskAlreadyRequested)
                {
                    Task.Run(() => DoPurge());
                }
            }
        }

        private void DoPurge()
        {
            List<Type>? toPurge = null;
            lock (_locker)
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
            lock (_locker)
            {
                if (!_subscriptions.TryGetValue(
                    type, out Dictionary<Guid, BaseSubscription> messageSubscriptions))
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

                MvxPluginLog.Instance?.Log(LogLevel.Trace, "Purging {0} subscriptions", deadSubscriptionIds.Count);
                foreach (var id in deadSubscriptionIds)
                {
                    messageSubscriptions.Remove(id);
                }

                PublishSubscriberChangeMessage(type, messageSubscriptions);
            }
        }
    }
#nullable restore
}

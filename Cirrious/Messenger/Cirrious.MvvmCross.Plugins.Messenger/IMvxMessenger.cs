// IMvxMessenger.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public interface IMvxMessenger
    {
        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="reference">Use a strong or weak reference to the deliveryAction</param>
        /// <param name="tag">An optional tag to include with this subscription</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        MvxSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
            where TMessage : MvxMessage;

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// This subscription always invokes the delivery Action on the Main thread.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="reference">Use a strong or weak reference to the deliveryAction</param>
        /// <param name="tag">An optional tag to include with this subscription</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        MvxSubscriptionToken SubscribeOnMainThread<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
             where TMessage : MvxMessage;

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// This subscription always invokes the delivery Action called on a threadpool thread.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="reference">Use a strong or weak reference to the deliveryAction</param>
        /// <param name="tag">An optional tag to include with this subscription</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        MvxSubscriptionToken SubscribeOnThreadPoolThread<TMessage>(Action<TMessage> deliveryAction, MvxReference reference = MvxReference.Weak, string tag = null)
             where TMessage : MvxMessage;

        /// <summary>
        /// Unsubscribe from a particular message type.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="mvxSubscriptionId">Subscription to remove</param>
        void Unsubscribe<TMessage>(MvxSubscriptionToken mvxSubscriptionId)
            where TMessage : MvxMessage;

        /// <summary>
        /// Has subscriptions for TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        bool HasSubscriptionsFor<TMessage>()
             where TMessage : MvxMessage;

        /// <summary>
        /// Number of subscriptions for TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        int CountSubscriptionsFor<TMessage>()
             where TMessage : MvxMessage;

        /// <summary>
        /// Has subscriptions for TMessage with a tag value of tag
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="tag">An optional tag to include with this subscription</param>
        /// <returns></returns>
        bool HasSubscriptionsForTag<TMessage>(string tag)
             where TMessage : MvxMessage;

        /// <summary>
        /// Number of subscriptions for TMessage with a tag value of tag
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="tag">An optional tag to include with this subscription</param>
        /// <returns></returns>
        int CountSubscriptionsForTag<TMessage>(string tag)
             where TMessage : MvxMessage;

        /// <summary>
        /// Get all the tags (including nulls) for subscriptions for TMessage
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <returns></returns>
        IList<string> GetSubscriptionTagsFor<TMessage>()
             where TMessage : MvxMessage;


        /// <summary>
        /// Publish a message to any subscribers
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        void Publish<TMessage>(TMessage message) where TMessage : MvxMessage;

        /// <summary>
        /// Publish a message to any subscribers
        /// - GetType() will be used to determine the message type
        /// </summary>
        /// <param name="message">Message to deliver</param>
        void Publish(MvxMessage message);

        /// <summary>
        /// Publish a message to any subscribers
        /// </summary>
        /// <param name="message">Message to deliver</param>
        /// <param name="messageType">The type of the message to use for delivery - message should be of that class or a of a subclass</param>
        void Publish(MvxMessage message, Type messageType);

        /// <summary>
        /// Schedules a check on all subscribers for the specified messageType. If any are not alive, they will be removed
        /// </summary>
        /// <param name="messageType">The type of the message to check</param>
        void RequestPurge(Type messageType);

        /// <summary>
        /// Schedules a check on all subscribers for all messageType. If any are not alive, they will be removed
        /// </summary>
        void RequestPurgeAll();
    }
}
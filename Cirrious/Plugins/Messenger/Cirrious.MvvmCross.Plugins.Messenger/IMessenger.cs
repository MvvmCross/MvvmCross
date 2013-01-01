#region Copyright

// <copyright file="IMessenger.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public interface IMessenger
    {
        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReference">Use a strong reference to the deliveryAction</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        Guid Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReference = false)
            where TMessage : BaseMessage;

        /// <summary>
        /// Unsubscribe from a particular message type.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="subscriptionId">Subscription to remove</param>
        void Unsubscribe<TMessage>(Guid subscriptionId)
            where TMessage : BaseMessage;

        /// <summary>
        /// Publish a message to any subscribers
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        void Publish<TMessage>(TMessage message) where TMessage : BaseMessage;
    }
}
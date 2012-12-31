#region Copyright

// <copyright file="TypedSubscription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class TypedSubscription<TMessage> : BaseSubscription
        where TMessage : BaseMessage
    {
        public override sealed bool Invoke(object message)
        {
            var typedMessage = message as TMessage;
            if (typedMessage == null)
                throw new MvxException("Unexpected message {0}", message);

            return TypedInvoke(typedMessage);
        }

        public abstract bool TypedInvoke(TMessage message);
    }
}
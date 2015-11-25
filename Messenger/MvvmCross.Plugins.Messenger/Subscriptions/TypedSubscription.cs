// TypedSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using MvvmCross.Plugins.Messenger.ThreadRunners;

namespace MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class TypedSubscription<TMessage> : BaseSubscription
        where TMessage : MvxMessage
    {
        protected TypedSubscription(IMvxActionRunner actionRunner, string tag)
            : base(actionRunner, tag)
        {
        }

        public sealed override bool Invoke(object message)
        {
            var typedMessage = message as TMessage;
            if (typedMessage == null)
                throw new MvxException("Unexpected message {0}", message);

            return TypedInvoke(typedMessage);
        }

        protected abstract bool TypedInvoke(TMessage message);
    }
}
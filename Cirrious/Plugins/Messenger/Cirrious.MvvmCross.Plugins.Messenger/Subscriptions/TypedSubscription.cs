using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Exceptions;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class TypedSubscription<TMessage> : BaseSubscription
        where TMessage : BaseMessage
    {
        public sealed override bool Invoke(object message)
        {
            var typedMessage = message as TMessage;
            if (typedMessage == null)
                throw new MvxException("Unexpected message {0}", message);

            return TypedInvoke(typedMessage);
        }

        public abstract bool TypedInvoke(TMessage message);
    }
}

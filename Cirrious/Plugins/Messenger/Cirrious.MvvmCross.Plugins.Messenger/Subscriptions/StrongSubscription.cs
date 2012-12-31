using System;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public class StrongSubscription<TMessage> : TypedSubscription<TMessage>
        where TMessage : BaseMessage
    {
        private readonly Action<TMessage> _action;

        public override bool IsAlive
        {
            get { return true; }
        }

        public override bool TypedInvoke(TMessage message)
        {
            _action(message);
            return true;
        }

        public StrongSubscription(Action<TMessage> action)
        {
            _action = action;
        }
    }
}
﻿using System;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public class WeakSubscription<TMessage> : TypedSubscription<TMessage>
        where TMessage : BaseMessage
    {
        private readonly WeakReference _weakReference;

        public override bool IsAlive
        {
            get { return _weakReference.IsAlive; }
        }

        public override bool TypedInvoke(TMessage message)
        {
            if (!_weakReference.IsAlive)
                return false;

            var action = _weakReference.Target as Action<TMessage>;
            if (action == null)
                return false;

            action(message);
            return true;
        }

        public WeakSubscription(Action<TMessage> listener)
        {
            _weakReference = new WeakReference(listener, false);
        }
    }
}
using System;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class BaseSubscription
    {
        public Guid Id { get; private set; }
        public abstract bool IsAlive { get; }
        public abstract bool Invoke(object message);

        protected BaseSubscription()
        {
            Id = Guid.NewGuid();
        }
    }
}
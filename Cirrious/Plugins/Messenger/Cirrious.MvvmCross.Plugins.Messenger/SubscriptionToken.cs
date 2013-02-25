using System;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class SubscriptionToken
    {
        public Guid Id { get; private set; }
        private readonly object[] _dependentObjects;

        public SubscriptionToken(Guid id, params object[] dependentObjects)
        {
            Id = id;
            _dependentObjects = dependentObjects;
        }
    }
}
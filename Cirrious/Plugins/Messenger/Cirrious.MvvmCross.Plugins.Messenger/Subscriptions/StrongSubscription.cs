// StrongSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
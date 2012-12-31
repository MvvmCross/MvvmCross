#region Copyright

// <copyright file="WeakSubscription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

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
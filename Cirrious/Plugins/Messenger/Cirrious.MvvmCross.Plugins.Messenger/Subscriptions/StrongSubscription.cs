#region Copyright

// <copyright file="StrongSubscription.cs" company="Cirrious">
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
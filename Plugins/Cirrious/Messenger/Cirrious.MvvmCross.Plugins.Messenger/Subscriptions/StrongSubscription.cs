﻿// StrongSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public class StrongSubscription<TMessage> : TypedSubscription<TMessage>
        where TMessage : MvxMessage
    {
        private readonly Action<TMessage> _action;

        public override bool IsAlive
        {
            get { return true; }
        }

        protected override bool TypedInvoke(TMessage message)
        {
            Call(() => _action(message));
            return true;
        }

        public StrongSubscription(IMvxActionRunner actionRunner, Action<TMessage> action, string tag)
            : base(actionRunner, tag)
        {
            _action = action;
        }
    }
}
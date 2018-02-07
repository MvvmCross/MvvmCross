// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Plugins.Messenger.ThreadRunners;

namespace MvvmCross.Plugin.Messenger.Subscriptions
{
    public class StrongSubscription<TMessage> : TypedSubscription<TMessage>
        where TMessage : MvxMessage
    {
        private readonly Action<TMessage> _action;

        public override bool IsAlive => true;

        protected override bool TypedInvoke(TMessage message)
        {
            Call(() => _action?.Invoke(message));
            return true;
        }

        public StrongSubscription(IMvxActionRunner actionRunner, Action<TMessage> action, string tag)
            : base(actionRunner, tag)
        {
            _action = action;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Plugins.Messenger.ThreadRunners;

namespace MvvmCross.Plugins.Messenger.Subscriptions
{
    public class WeakSubscription<TMessage> : TypedSubscription<TMessage>
        where TMessage : MvxMessage
    {
        private readonly WeakReference _weakReference;

        public override bool IsAlive => _weakReference.IsAlive;

        protected override bool TypedInvoke(TMessage message)
        {
            if (!_weakReference.IsAlive)
                return false;

            var action = _weakReference.Target as Action<TMessage>;
            if (action == null)
                return false;

            Call(() =>
            {
                action?.Invoke(message);
            });
            return true;
        }

        public WeakSubscription(IMvxActionRunner actionRunner, Action<TMessage> listener, string tag)
            : base(actionRunner, tag)
        {
            _weakReference = new WeakReference(listener);
        }
    }
}
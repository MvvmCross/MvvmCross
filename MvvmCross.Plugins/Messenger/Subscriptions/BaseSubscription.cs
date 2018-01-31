// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Plugins.Messenger.ThreadRunners;

namespace MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class BaseSubscription
    {
        public Guid Id { get; private set; }
        public string Tag { get; private set; }
        public abstract bool IsAlive { get; }

        public abstract bool Invoke(object message);

        private readonly IMvxActionRunner _actionRunner;

        protected BaseSubscription(IMvxActionRunner actionRunner, string tag)
        {
            _actionRunner = actionRunner;
            Id = Guid.NewGuid();
            Tag = tag;
        }

        protected void Call(Action action)
        {
            _actionRunner.Run(action);
        }
    }
}
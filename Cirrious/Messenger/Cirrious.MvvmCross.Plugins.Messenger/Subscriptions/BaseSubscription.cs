// BaseSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Plugins.Messenger.ThreadRunners;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class BaseSubscription
    {
        public Guid Id { get; private set; }
        public abstract bool IsAlive { get; }
        public abstract bool Invoke(object message);

        private readonly IMvxActionRunner _actionRunner;

        protected BaseSubscription(IMvxActionRunner actionRunner)
        {
            _actionRunner = actionRunner;
            Id = Guid.NewGuid();
        }

        protected void Call(Action action)
        {
            _actionRunner.Run(action);
        }
    }
}
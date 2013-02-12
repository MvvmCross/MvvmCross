// TypedSubscription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Plugins.Messenger.Subscriptions
{
    public abstract class TypedSubscription<TMessage> : BaseSubscription, IMvxServiceConsumer
        where TMessage : BaseMessage
    {
        public override sealed bool Invoke(object message)
        {
            var typedMessage = message as TMessage;
            if (typedMessage == null)
                throw new MvxException("Unexpected message {0}", message);

            return TypedInvoke(typedMessage);
        }

        protected bool InvokeOnMainThread(Action stuff)
        {
            var dispatcherProvider = this.GetService<IMvxViewDispatcherProvider>();
            var dispatcher = dispatcherProvider.Dispatcher;
            return dispatcher != null && dispatcher.RequestMainThreadAction(stuff);
        }

        public abstract bool TypedInvoke(TMessage message);

        public override bool IsUiThreadSubscription { get; set; }
    }
}
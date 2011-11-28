using System;

namespace Phone7.Fx.Messaging
{
    public class DispatcherChannelSubscription<TPayload> : ChannelSubscription<TPayload>
    {
        private readonly IDispatcherFacade _dispatcher;

        public DispatcherChannelSubscription(DelegateReference actionReference, DelegateReference filterReference, IDispatcherFacade dispatcher) : base(actionReference, filterReference)
        {
            _dispatcher = dispatcher;
        }

        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            _dispatcher.BeginInvoke(action, argument);
        }
    }
}
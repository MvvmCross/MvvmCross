using System;

namespace Phone7.Fx.Messaging
{
    public class ChannelSubscription<TPayload> 
    {
        private readonly DelegateReference _actionReference;
        private readonly DelegateReference _filterReference;

       
        public ChannelSubscription(DelegateReference actionReference, DelegateReference filterReference)
        {
            if (actionReference == null)
                throw new ArgumentNullException("actionReference");
            if (!(actionReference.Target is Action<TPayload>))
                throw new ArgumentException("InvalidDelegateRerefenceTypeException", "actionReference");

            if (filterReference == null)
                throw new ArgumentNullException("filterReference");
            if (!(filterReference.Target is Predicate<TPayload>))
                throw new ArgumentException("InvalidDelegateRerefenceTypeExceptio", "filterReference");

            _actionReference = actionReference;
            _filterReference = filterReference;
        }

        public Action<TPayload> Action
        {
            get { return (Action<TPayload>)_actionReference.Target; }
        }

      
        public Predicate<TPayload> Filter
        {
            get { return (Predicate<TPayload>)_filterReference.Target; }
        }

     
        public ChannelSubscriptionToken SubscriptionToken { get; set; }

      
        public virtual Action<object[]> GetExecutionStrategy()
        {
            Action<TPayload> action = this.Action;
            Predicate<TPayload> filter = this.Filter;
            if (action != null && filter != null)
            {
                return arguments =>
                {
                    TPayload argument = default(TPayload);
                    if (arguments != null && arguments.Length > 0 && arguments[0] != null)
                    {
                        argument = (TPayload)arguments[0];
                    }
                    if (filter(argument))
                    {
                        InvokeAction(action, argument);
                    }
                };
            }
            return null;
        }

      
        public virtual void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            action(argument);
        }
    }
}
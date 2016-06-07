// MvxWithEventPropertyInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;
    using System.Reflection;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Platform;
    using MvvmCross.Platform.WeakSubscription;

    public class MvxWithEventPropertyInfoTargetBinding
        : MvxPropertyInfoTargetBinding
    {
        private IDisposable _subscription;

        public MvxWithEventPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - target is null in MvxWithEventPropertyInfoTargetBinding");
                return;
            }
        }

        // Note - this is public because we use it in weak referenced situations
        public void OnValueChanged(object sender, EventArgs eventArgs)
        {
            var target = this.Target;
            if (target == null)
            {
                MvxBindingTrace.Trace("Null weak reference target seen during OnValueChanged - unusual as usually Target is the sender of the value changed. Ignoring the value changed");
                return;
            }

            var value = this.TargetPropertyInfo.GetGetMethod().Invoke(target, null);
            this.FireValueChanged(value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = this.Target;
            if (target == null)
                return;

            var viewType = target.GetType();
            var eventName = this.TargetPropertyInfo.Name + "Changed";
            var eventInfo = viewType.GetEvent(eventName);
            if (eventInfo == null)
            {
                // this will be a one way binding
                return;
            }

            if (eventInfo.EventHandlerType != typeof(EventHandler))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Diagnostic - cannot two-way bind to {0}/{1} on type {2} because eventHandler is type {3}",
                                      viewType,
                                      eventName,
                                      target.GetType().Name,
                                      eventInfo.EventHandlerType.Name);
                return;
            }

            this._subscription = eventInfo.WeakSubscribe(target, OnValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (this._subscription != null)
                {
                    this._subscription.Dispose();
                    this._subscription = null;
                }
            }

            base.Dispose(isDisposing);
        }
    }
}
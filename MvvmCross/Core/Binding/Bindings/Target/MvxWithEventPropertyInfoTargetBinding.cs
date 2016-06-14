// MvxWithEventPropertyInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Target
{
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
            }
        }

        protected string EventSuffix { get; set; } = "Changed";

        // Note - this is public because we use it in weak referenced situations
        public void OnValueChanged(object sender, EventArgs eventArgs)
        {
            var target = Target;
            if (target == null)
            {
                MvxBindingTrace.Trace("Null weak reference target seen during OnValueChanged - unusual as usually Target is the sender of the value changed. Ignoring the value changed");
                return;
            }

            var value = TargetPropertyInfo.GetGetMethod().Invoke(target, null);
            FireValueChanged(value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = Target;
            if (target == null)
                return;

            var viewType = target.GetType();
            var eventName = TargetPropertyInfo.Name + EventSuffix;
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

            _subscription = eventInfo.WeakSubscribe(target, OnValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }

            base.Dispose(isDisposing);
        }
    }

    public class MvxWithEventPropertyInfoTargetBinding<T> : MvxWithEventPropertyInfoTargetBinding
        where T : class
    {
        public MvxWithEventPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected T View => Target as T;
    }
}
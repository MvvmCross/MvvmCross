// MvxWithEventPropertyInfoTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.WeakSubscription;
using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
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
                return;
            }
        }

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
            var eventName = TargetPropertyInfo.Name + "Changed";
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
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }
        }
    }
}
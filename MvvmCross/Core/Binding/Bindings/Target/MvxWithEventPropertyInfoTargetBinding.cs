// MvxWithEventPropertyInfoTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.ComponentModel;
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
        
        // Note - this is public because we use it in weak referenced situations
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            var target = Target;
            if (target == null)
            {
                MvxBindingTrace.Trace("Null weak reference target seen during OnPropertyChanged - unusual as usually Target is the sender of the value changed. Ignoring the value changed");
                return;
            }

            if (eventArgs.PropertyName == TargetPropertyInfo.Name)
            {
                var value = TargetPropertyInfo.GetGetMethod().Invoke(target, null);
                FireValueChanged(value);
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var target = Target;
            if (target == null)
                return;

            var viewType = target.GetType();

            // Try a specific [PropertyName]Changed event first.
            var eventInfo = GetNamedPropertyChangedEvent(viewType, TargetPropertyInfo.Name);
            if (eventInfo != null)
                _subscription = eventInfo.WeakSubscribe(target, OnValueChanged);
            else if (target is INotifyPropertyChanged notifyPropertyChangedTarget)
            {
                // If target implements INPC then try a generic PropertyChanged event next.
                eventInfo = GetPropertyChangedEvent(viewType);
                if (eventInfo != null)
                    _subscription = notifyPropertyChangedTarget.WeakSubscribe(OnPropertyChanged);
            }

            // No suitable event found on target; this will be a one way binding.
        }

        private EventInfo GetNamedPropertyChangedEvent(Type viewType, string propertyName)
        {
            var eventName = propertyName + "Changed";
            var eventInfo = viewType.GetEvent(eventName);

            if (eventInfo == null)
                return null;

            if (eventInfo.EventHandlerType != typeof(EventHandler))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Diagnostic - cannot two-way bind to {0}/{1} on type {2} because eventHandler is type {3}",
                                      viewType,
                                      eventName,
                                      viewType.Name,
                                      eventInfo.EventHandlerType.Name);
                return null;
            }

            return eventInfo;
        }

        private EventInfo GetPropertyChangedEvent(Type viewType)
        {
            var eventName = "PropertyChanged";
            var eventInfo = viewType.GetEvent(eventName);

            if (eventInfo == null)
                return null;

            if (eventInfo.EventHandlerType != typeof(PropertyChangedEventHandler))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Diagnostic,
                                      "Diagnostic - cannot two-way bind to {0}/{1} on type {2} because eventHandler is type {3}",
                                      viewType,
                                      eventName,
                                      viewType.Name,
                                      eventInfo.EventHandlerType.Name);
                return null;
            }

            return eventInfo;
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
}
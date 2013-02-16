using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public class MvxWithEventPropertyInfoTargetBinding : MvxPropertyInfoTargetBinding
    {
        private readonly EventInfo _changedEventInfo;

        public MvxWithEventPropertyInfoTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            if (target == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - target is null in MvxWithEventPropertyInfoTargetBinding");
            }
            else
            {
                var viewType = target.GetType();
                var eventName = targetPropertyInfo.Name + "Changed";
                var eventInfo = viewType.GetEvent(eventName);
                if (eventInfo == null)
                {
                    // this will be a one way binding
                    return;
                }
                if (eventInfo.EventHandlerType != typeof (EventHandler))
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                          "Warning - cannot bind to ValueChanged on type {0} because eventHandler is type {1}", target.GetType().Name, eventInfo.EventHandlerType.Name);
                    return;
                }

                _changedEventInfo = eventInfo;
                _changedEventInfo.AddEventHandler(target, new EventHandler(OnValueChanged));
            }
        }

        private void OnValueChanged(object sender, EventArgs eventArgs)
        {
            var value = GetValueByReflection();
            FireValueChanged(value);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return _changedEventInfo == null ? MvxBindingMode.OneWay : MvxBindingMode.TwoWay;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                if (_changedEventInfo != null)
                {
                    var remove = _changedEventInfo.GetRemoveMethod();
                    var view = Target;
                    if (view != null)
                        remove.Invoke(view, null);
                }
            }
        }
    }
}
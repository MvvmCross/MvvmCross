using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Dialog.Touch.Target
{
    public class MvxValueElementValueBinding<TValueType> : MvxPropertyInfoTargetBinding<ValueElement<TValueType>>
    {
        public MvxValueElementValueBinding(object target, PropertyInfo targetPropertyInfo) 
            : base(target, targetPropertyInfo)
        {
            var valueElement = View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - valueElement is null in MvxValueElementValueBinding");
            }
            else
            {
                valueElement.ValueChanged += ValueElementOnValueChanged;
            }
        }

        private void ValueElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.Value);
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var valueElement = View;
                if (valueElement != null)
                {
                    valueElement.ValueChanged -= ValueElementOnValueChanged;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
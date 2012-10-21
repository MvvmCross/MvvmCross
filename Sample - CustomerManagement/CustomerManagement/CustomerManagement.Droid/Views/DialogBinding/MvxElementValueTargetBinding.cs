using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using FooBar.Dialog.Droid;

namespace CustomerManagement.Droid.Views
{
    public class MvxElementValueTargetBinding : MvxPropertyInfoTargetBinding<ValueElement>
    {
        public MvxElementValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var valueElement = View;
            if (valueElement == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - ValueElement is null in MvxEditTextTextTargetBinding");
            }
            else
            {
                valueElement.ValueChanged += ElementOnValueChanged;
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        private void ElementOnValueChanged(object sender, EventArgs eventArgs)
        {
            FireValueChanged(View.Value);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var editText = View;
                if (editText != null)
                {
                    editText.ValueChanged -= ElementOnValueChanged;
                }
            }
        }
    }
}
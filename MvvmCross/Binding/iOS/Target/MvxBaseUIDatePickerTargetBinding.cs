// MvxBaseUIDatePickerTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public abstract class MvxBaseUIDatePickerTargetBinding : MvxPropertyInfoTargetBinding<UIDatePicker>
    {
        protected MvxBaseUIDatePickerTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var datePicker = View;
            if (datePicker == null)
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                    "Error - UIDatePicker is null in MvxBaseUIDatePickerTargetBinding");
            else
                datePicker.ValueChanged += DatePickerOnValueChanged;
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        private void DatePickerOnValueChanged(object sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(GetValueFrom(view));
        }

        protected abstract object GetValueFrom(UIDatePicker view);

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var datePicker = View;
                if (datePicker != null)
                    datePicker.ValueChanged -= DatePickerOnValueChanged;
            }
        }
    }
}
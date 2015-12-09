// MvxBaseUIDatePickerTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
{
    using System;
    using System.Reflection;

    using MvvmCross.Platform.Platform;

    using UIKit;

    public abstract class MvxBaseUIDatePickerTargetBinding : MvxPropertyInfoTargetBinding<UIDatePicker>
    {
        protected MvxBaseUIDatePickerTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var datePicker = View;
            if (datePicker == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UIDatePicker is null in MvxBaseUIDatePickerTargetBinding");
            }
            else
            {
                datePicker.ValueChanged += DatePickerOnValueChanged;
            }
        }

        private void DatePickerOnValueChanged(object sender, EventArgs eventArgs)
        {
            var view = View;
            if (view == null)
                return;
            FireValueChanged(this.GetValueFrom(view));
        }

        protected abstract object GetValueFrom(UIDatePicker view);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var datePicker = View;
                if (datePicker != null)
                {
                    datePicker.ValueChanged -= DatePickerOnValueChanged;
                }
            }
        }
    }
}
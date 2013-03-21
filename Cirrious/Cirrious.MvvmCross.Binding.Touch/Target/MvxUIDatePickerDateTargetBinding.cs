// MvxUIDatePickerDateTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIDatePickerDateTargetBinding : MvxPropertyInfoTargetBinding<UIDatePicker>
    {
        public MvxUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var datePicker = View;
            if (datePicker == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - UIDatePicker is null in MvxUIDatePickerDateTargetBinding");
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
            FireValueChanged((DateTime) view.Date);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override object MakeSafeValue(object value)
        {
            var date = (DateTime) value;
            NSDate nsDate = date;
            return nsDate;
        }

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
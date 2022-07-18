// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using Foundation;

namespace MvvmCross.Platforms.Mac.Binding.Target
{
    public class MvxNSDatePickerDateTargetBinding : MvxBaseNSDatePickerTargetBinding
    {
        public MvxNSDatePickerDateTargetBinding(NSDatePicker datePicker)
            : base(datePicker)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var datePicker = this.DatePicker;
            if (datePicker == null)
                return;

            // sets DateValue to the GMT value of DateTime, but the UI will show the correct time
            // Note: Probably we should not use DateTime, but instead DateTimeOffset or something else that identifies timezone
            datePicker.DateValue = (NSDate)((DateTime)value);
        }

        public override Type TargetValueType
        {
            get { return typeof(DateTime); }
        }

        protected override object GetValueFrom(NSDatePicker view)
        {
            return this.GetLocalTime(view);
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = DateTime.Now;
            var date = (DateTime)value;
            return date;
        }
    }
}

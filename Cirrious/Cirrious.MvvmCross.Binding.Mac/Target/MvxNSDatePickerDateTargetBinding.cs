// MvxUIDatePickerDateTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
    public class MvxNSDatePickerDateTargetBinding : MvxBaseNSDatePickerTargetBinding
    {
        public MvxNSDatePickerDateTargetBinding(NSDatePicker datePicker)
            : base(datePicker)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var datePicker = DatePicker;
            if (datePicker == null)
                return;

            // sets DateValue to the GMT value of DateTime, but the UI will show the correct time
            // Note: Probably we should not use DateTime, but instead DateTimeOffset or something else that identifies timezone
            datePicker.DateValue = (NSDate)((DateTime)value);
        }

        public override Type TargetType
        {
            get { return typeof(DateTime); }
        }

        protected override object GetValueFrom(NSDatePicker view)
        {
            return GetLocalTime(view);
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
// MvxUIDatePickerTimeTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


namespace MvvmCross.Binding.Mac.Target
{
    using System;

    using AppKit;
    using Foundation;

    public class MvxNSDatePickerTimeTargetBinding : MvxBaseNSDatePickerTargetBinding
    {
        public MvxNSDatePickerTimeTargetBinding(NSDatePicker datePicker)
            : base(datePicker)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var picker = this.DatePicker;
            if (picker == null)
                return;

            var time = (DateTime)value;

            // Do this in a way that does not mess up the date, grab current date, then modify the time
            var pickerDate = this.GetLocalTime(this.DatePicker);
            var date = new DateTime(pickerDate.Year, pickerDate.Month, pickerDate.Day,
                time.Hour, time.Minute, time.Second, DateTimeKind.Local);

            //var date = new DateTime (2000, 1, 1).Add (timespan);
            picker.DateValue = (NSDate)(date);
        }

        protected override object GetValueFrom(NSDatePicker view)
        {
            var date = this.GetLocalTime(view);
            return date.TimeOfDay;

            //			var components = NSCalendar.CurrentCalendar.Components(
            //				NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second,
            //				view.DateValue);
            //            return new TimeSpan((int)components.Hour, (int)components.Minute, (int)components.Second);
        }

        public override Type TargetType
        {
            get { return typeof(TimeSpan); }
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = TimeSpan.FromSeconds(0);
            var time = (TimeSpan)value;
            var now = DateTime.Now;
            var date = new DateTime(
                2000,
                1,
                1,
                time.Hours,
                time.Minutes,
                time.Seconds,
                DateTimeKind.Local);

            return date;
        }
    }
}
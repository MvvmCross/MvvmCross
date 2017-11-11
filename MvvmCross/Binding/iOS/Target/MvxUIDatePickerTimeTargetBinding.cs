// MvxUIDatePickerTimeTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Platform.iOS;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIDatePickerTimeTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerTimeTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            // Convert from universal NSDate back to a local DateTime based on system timezone, and return its time of day.
            var valueUtc = view.Date.ToDateTimeUtc();
            var valueLocal = ToLocalTime(valueUtc);
            return valueLocal.TimeOfDay;
        }

        //public override Type TargetType => typeof(TimeSpan);

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = TimeSpan.FromSeconds(0);

            var time = (TimeSpan)value;
            var now = DateTime.Now;

            // Convert from local DateTime with the TimeSpan as its time of day to universal NSDate based on system timezone.

            var dateLocal = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                time.Hours,
                time.Minutes,
                time.Seconds,
                DateTimeKind.Local);

            var dateUtc = ToUtcTime(dateLocal);
            var nsDate = dateUtc.ToNSDate();

            return nsDate;
        }
    }
}
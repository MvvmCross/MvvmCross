// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Platforms.Ios;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Target
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

        //public override Type TargetValueType => typeof(TimeSpan);

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

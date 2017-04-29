// MvxUIDatePickerTimeTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Foundation;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIDatePickerTimeTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerTimeTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override Type TargetType => typeof(TimeSpan);

        protected override object GetValueFrom(UIDatePicker view)
        {
            var components = NSCalendar.CurrentCalendar.Components(
                NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second,
                view.Date);
            return new TimeSpan((int) components.Hour, (int) components.Minute, (int) components.Second);
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = TimeSpan.FromSeconds(0);
            var time = (TimeSpan) value;
            var now = DateTime.Now;
            var date = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                time.Hours,
                time.Minutes,
                time.Seconds,
                DateTimeKind.Local);

            var nsDate = (NSDate) date;
            return nsDate;
        }
    }
}
// MvxUIDatePickerDateTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Foundation;
using MvvmCross.Platform.iOS;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIDatePickerDateTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            // Convert from universal NSDate back to a local DateTime based on system timezone.
            var valueUtc = view.Date.ToDateTimeUtc();
            var valueLocal = ToLocalTime(valueUtc);
            return valueLocal;
        }

        public static DateTime DefaultDate { get; set; } = DateTime.Now;

        protected override object MakeSafeValue(object value)
        {
            // Convert from local DateTime (or default value) to universal NSDate based on system timezone.
            var valueLocal = (DateTime)(value ?? DefaultDate);
            var valueUtc = ToUtcTime(valueLocal);
            var valueNSDate = valueUtc.ToNSDate();

            if (View.MaximumDate != null && View.MaximumDate.Compare(valueNSDate) == NSComparisonResult.Ascending)
                valueNSDate = View.MaximumDate;
            else if (View.MinimumDate != null && View.MinimumDate.Compare(valueNSDate) == NSComparisonResult.Descending)
                valueNSDate = View.MinimumDate;

            return valueNSDate;
        }
    }
}
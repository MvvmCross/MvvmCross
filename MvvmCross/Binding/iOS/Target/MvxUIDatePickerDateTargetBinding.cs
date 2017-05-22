// MvxUIDatePickerDateTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.iOS.Target
{
    using System;
    using System.Reflection;

    using Foundation;

    using UIKit;

    public class MvxUIDatePickerDateTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return (new DateTime(2001, 1, 1, 0, 0, 0)).AddSeconds(view.Date.SecondsSinceReferenceDate);
        }

        public static DateTime DefaultDate { get; set; } = DateTime.Now;

        protected override object MakeSafeValue(object value)
        {
            var date = (DateTime)(value ?? DefaultDate);

            if (View.MaximumDate != null && date > (DateTime)View.MaximumDate)
                date = (DateTime)View.MaximumDate;
            else if (View.MinimumDate != null && date < (DateTime)View.MinimumDate)
                date = (DateTime)View.MinimumDate;

            return (NSDate)date;
        }
    }
}
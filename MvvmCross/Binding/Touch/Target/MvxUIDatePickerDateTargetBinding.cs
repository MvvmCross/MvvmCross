// MvxUIDatePickerDateTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Target
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

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = DateTime.UtcNow;
            var date = (DateTime)value;
            var nsDate = NSDate.FromTimeIntervalSinceReferenceDate((date - (new DateTime(2001, 1, 1, 0, 0, 0))).TotalSeconds);
            return nsDate;
        }
    }
}
// MvxUIDatePickerMinMaxTargetBinding.cs

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
    public class MvxUIDatePickerMinMaxTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerMinMaxTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var targetPropertyName = targetPropertyInfo.Name;
            if (targetPropertyName == nameof(UIDatePicker.Date))
                throw new ArgumentException("This binding cannot be used with the Date property as the target.");
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            // This method should never be called.
            throw new NotImplementedException();
        }

        protected override object MakeSafeValue(object value)
        {
            var valueUtc = ToUtcTime((DateTime)value);
            var valueNSDate = valueUtc.ToNSDate();

            return valueNSDate;
        }
    }
}
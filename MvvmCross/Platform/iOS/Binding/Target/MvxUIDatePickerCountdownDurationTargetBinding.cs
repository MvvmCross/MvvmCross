// MvxUIDatePickerTimeTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public class MvxUIDatePickerCountDownDurationTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerCountDownDurationTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return view.CountDownDuration;
        }

        public override Type TargetType => typeof(double);
    }
}
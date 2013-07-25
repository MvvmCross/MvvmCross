// MvxUIDatePickerDateTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Target
{
    public class MvxUIDatePickerDateTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return ((DateTime) view.Date).Date;
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = DateTime.UtcNow;
            var date = (DateTime) value;
            NSDate nsDate = date;
            return nsDate;
        }
    }
}
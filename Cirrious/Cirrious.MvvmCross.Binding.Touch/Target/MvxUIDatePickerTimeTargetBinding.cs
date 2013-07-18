// MvxUIDatePickerTimeTargetBinding.cs
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
    public class MvxUIDatePickerTimeTargetBinding : MvxBaseUIDatePickerTargetBinding
    {
        public MvxUIDatePickerTimeTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        protected override object GetValueFrom(UIDatePicker view)
        {
            return ((DateTime) view.Date).TimeOfDay;
        }

        public override Type TargetType
        {
            get { return typeof(TimeSpan); }
        }

        protected override object MakeSafeValue(object value)
        {
            if (value == null)
                value = TimeSpan.FromSeconds(0);
            var time = (TimeSpan) value;
            var date = new DateTime(2000, 1, 1, 1, 0, 0, 0, DateTimeKind.Local).Add(time);
            NSDate nsDate = date;
            return nsDate;
        }
    }
}
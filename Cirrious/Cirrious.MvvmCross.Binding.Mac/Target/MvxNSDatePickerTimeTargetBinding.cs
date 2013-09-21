// MvxUIDatePickerTimeTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
	public class MvxNSDatePickerTimeTargetBinding : MvxBaseNSDatePickerTargetBinding
	{
		public MvxNSDatePickerTimeTargetBinding(object target, PropertyInfo targetPropertyInfo)
			: base(target, targetPropertyInfo)
		{
		}

		protected override object GetValueFrom(NSDatePicker view)
		{
			var components = NSCalendar.CurrentCalendar.Components(
				NSCalendarUnit.Hour | NSCalendarUnit.Minute | NSCalendarUnit.Second, 
				view.DateValue);
			return new TimeSpan(components.Hour, components.Minute, components.Second);
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
			var now = DateTime.Now;
			var date = new DateTime(
				now.Year,
				now.Month,
				now.Day,
				time.Hours,
				time.Minutes,
				time.Seconds,
				DateTimeKind.Local);

			return date;
		}
	}
}
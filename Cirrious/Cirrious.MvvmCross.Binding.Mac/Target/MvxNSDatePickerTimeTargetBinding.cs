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
		public MvxNSDatePickerTimeTargetBinding(NSDatePicker datePicker)
			: base(datePicker)
		{
		}

		protected override void SetValueImpl (object target, object value)
		{
			var picker = DatePicker;
			if (picker == null)
				return;

			//var timespan = (TimeSpan)value;
			//var date = new DateTime (2000, 1, 1).Add (timespan);
			picker.DateValue = (DateTime)value;
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
				2000,
				1,
				1,
				time.Hours,
				time.Minutes,
				time.Seconds,
				DateTimeKind.Local);

			return date;
		}
	}
}
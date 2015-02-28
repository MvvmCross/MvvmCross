// MvxUIDatePickerDateTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
	public class MvxNSDatePickerDateTargetBinding : MvxBaseNSDatePickerTargetBinding
	{
		public MvxNSDatePickerDateTargetBinding(NSDatePicker datePicker)
			: base(datePicker)
		{
		}

		protected override void SetValueImpl(object target, object value)
		{
			var datePicker = DatePicker;
			if (datePicker == null)
				return;

			// sets DateValue to the GMT value of DateTime, but the UI will show the correct time
			datePicker.DateValue = (NSDate)((DateTime)value);
		}

		public override Type TargetType
		{
			get { return typeof(DateTime); }
		}

		protected override object GetValueFrom(NSDatePicker view)
		{
			var tzInfo = TimeZoneInfo.Local;
			return TimeZoneInfo.ConvertTimeFromUtc ((DateTime)view.DateValue, tzInfo);
	//		return ((DateTime) view.DateValue);
		}

		protected override object MakeSafeValue(object value)
		{
			if (value == null)
				value = DateTime.Now;
			var date = (DateTime) value;
			return date;
		}
	}
}
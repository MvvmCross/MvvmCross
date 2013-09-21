// MvxUIDatePickerDateTargetBinding.cs
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
	public class MvxNSDatePickerDateTargetBinding : MvxBaseNSDatePickerTargetBinding
	{
		public MvxNSDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
			: base(target, targetPropertyInfo)
		{
		}

		protected override object GetValueFrom(NSDatePicker view)
		{
			return ((DateTime) view.DateValue).Date;
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
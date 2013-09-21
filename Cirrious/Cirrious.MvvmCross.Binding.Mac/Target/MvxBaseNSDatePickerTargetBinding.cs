// MvxBaseUIDatePickerTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Binding.Mac.Target
{
	public abstract class MvxBaseNSDatePickerTargetBinding : MvxPropertyInfoTargetBinding<NSDatePicker>
	{
		protected MvxBaseNSDatePickerTargetBinding(object target, PropertyInfo targetPropertyInfo)
			: base(target, targetPropertyInfo)
		{
			var datePicker = View;
			if (datePicker == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error,
				                      "Error - NSDatePicker is null in MvxBaseNSDatePickerTargetBinding");
			}
			else
			{
				datePicker.Action = new MonoMac.ObjCRuntime.Selector ("datePickerAction:");
			}
		}

		[Export("datePickerAction:")]
		private void datePickerAction()
		{
			var view = View;
			if (view == null)
				return;
			FireValueChanged(GetValueFrom(view));
		}

		protected abstract object GetValueFrom(NSDatePicker view);

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.TwoWay; }
		}

		protected override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				var datePicker = View;
				if (datePicker != null)
				{
//					datePicker.ValueChanged -= DatePickerOnValueChanged;
				}
			}
		}
	}
}
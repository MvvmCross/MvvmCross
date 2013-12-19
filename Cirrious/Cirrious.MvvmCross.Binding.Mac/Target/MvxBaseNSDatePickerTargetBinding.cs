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
	public abstract class MvxBaseNSDatePickerTargetBinding : MvxMacTargetBinding
	{
		private bool _subscribed;

		protected NSDatePicker DatePicker 
		{		
			get { return base.Target as NSDatePicker; }
		}

		protected MvxBaseNSDatePickerTargetBinding(NSDatePicker target)
			: base(target)
		{
		}

		public override void SubscribeToEvents ()
		{
			var datePicker = DatePicker;

			if (datePicker == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error,
				                      "Error - NSDatePicker is null in MvxBaseNSDatePickerTargetBinding");
				return;
			}
			datePicker.Activated += HandleActivated;
			_subscribed = true;
		}

		void HandleActivated (object sender, EventArgs e)
		{
			var view = DatePicker;
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
				var datePicker = DatePicker;
				if (datePicker != null && _subscribed)
				{
					datePicker.Activated -= HandleActivated;
					_subscribed = false;
				}
			}
		}
	}
}
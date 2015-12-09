// MvxBaseUIDatePickerTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace MvvmCross.Binding.Mac.Target
{
    using System;

    using global::MvvmCross.Platform.Platform;

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

        public override void SubscribeToEvents()
        {
            var datePicker = this.DatePicker;

            if (datePicker == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error,
                                      "Error - NSDatePicker is null in MvxBaseNSDatePickerTargetBinding");
                return;
            }
            datePicker.Activated += HandleActivated;
            this._subscribed = true;
        }

        private void HandleActivated(object sender, EventArgs e)
        {
            var view = this.DatePicker;
            if (view == null)
                return;
            FireValueChanged(this.GetValueFrom(view));
        }

        protected abstract object GetValueFrom(NSDatePicker view);

        protected DateTime GetLocalTime(NSDatePicker view)
        {
            var tzInfo = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc((DateTime)view.DateValue, tzInfo);
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var datePicker = this.DatePicker;
                if (datePicker != null && this._subscribed)
                {
                    datePicker.Activated -= HandleActivated;
                    this._subscribed = false;
                }
            }
        }
    }
}
// MvxBaseUIDatePickerTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using Foundation;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.iOS;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;
using UIKit;

namespace MvvmCross.Binding.iOS.Target
{
    public abstract class MvxBaseUIDatePickerTargetBinding : MvxPropertyInfoTargetBinding<UIDatePicker>
    {
        private readonly NSTimeZone _systemTimeZone;
        private MvxWeakEventSubscription<UIDatePicker> _subscription;

        protected MvxBaseUIDatePickerTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            _systemTimeZone = NSTimeZone.SystemTimeZone;
        }

        private void DatePickerOnValueChanged(object sender, EventArgs args)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(GetValueFrom(view));
        }

        protected abstract object GetValueFrom(UIDatePicker view);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var datePicker = View;
            if (datePicker == null)
            {
                MvxBindingTrace.Error( "Error - UIDatePicker is null in MvxBaseUIDatePickerTargetBinding");
            }
            // Only listen for value changes if we are binding against one of the value-derived properties.
            else if (TargetPropertyInfo.Name == nameof(UIDatePicker.Date) || TargetPropertyInfo.Name == nameof(UIDatePicker.CountDownDuration))
            {
                _subscription = datePicker.WeakSubscribe(nameof(datePicker.ValueChanged), DatePickerOnValueChanged);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (!isDisposing) return;

            _subscription?.Dispose();
        }

        protected DateTime ToLocalTime(DateTime utc)
        {
            if (utc.Kind == DateTimeKind.Local)
                return utc;

            var local = utc.AddSeconds(_systemTimeZone.SecondsFromGMT(utc.ToNSDate())).WithKind(DateTimeKind.Local);

            return local;
        }

        protected DateTime ToUtcTime(DateTime local)
        {
            if (local.Kind == DateTimeKind.Utc)
                return local;

            var utc = local.AddSeconds(-_systemTimeZone.SecondsFromGMT(local.ToNSDate())).WithKind(DateTimeKind.Utc);

            return utc;
        }
    }
}
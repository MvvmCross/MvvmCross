// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxSpinnerSelectedItemBinding
        : MvxAndroidTargetBinding
    {
        protected MvxSpinner Spinner => (MvxSpinner)Target;

        private object _currentValue;
        private IDisposable _subscription;

        public MvxSpinnerSelectedItemBinding(MvxSpinner spinner)
            : base(spinner)
        {
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            var newValue = spinner.Adapter.GetRawItem(e.Position);

            bool changed;
            if (newValue == null)
            {
                changed = _currentValue != null;
            }
            else
            {
                changed = !newValue.Equals(_currentValue);
            }

            if (!changed)
            {
                return;
            }

            _currentValue = newValue;
            FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var spinner = (MvxSpinner)target;

            if (value == null)
            {
                MvxBindingLog.Warning("Null values not permitted in spinner SelectedItem binding currently");
                return;
            }

            if (!value.Equals(_currentValue))
            {
                var index = spinner.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingLog.Warning("Value not found for spinner {0}", value.ToString());
                    return;
                }
                _currentValue = value;
                spinner.SetSelection(index);
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            _subscription = spinner.WeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
                nameof(spinner.ItemSelected),
                SpinnerItemSelected);
        }

        public override Type TargetValueType => typeof(object);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}

// MvxAppCompatSpinnerSelectedItemBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Platform.Droid.WeakSubscription;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxAppCompatSpinnerSelectedItemBinding
        : MvxAndroidTargetBinding
    {
        protected MvxAppCompatSpinner Spinner => (MvxAppCompatSpinner)Target;

        private IDisposable _subscription;
        private object _currentValue;

        public MvxAppCompatSpinnerSelectedItemBinding(MvxAppCompatSpinner spinner)
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
            var spinner = (MvxAppCompatSpinner)target;

            if (value == null)
            {
                MvxBindingTrace.Warning("Null values not permitted in spinner SelectedItem binding currently");
                return;
            }

            if (!value.Equals(_currentValue))
            {
                var index = spinner.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
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

            _subscription = spinner.WeakSubscribe<MvxAppCompatSpinner, AdapterView.ItemSelectedEventArgs>(
                nameof(spinner.ItemSelected),
                SpinnerItemSelected);
        }

        public override Type TargetType => typeof(object);

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
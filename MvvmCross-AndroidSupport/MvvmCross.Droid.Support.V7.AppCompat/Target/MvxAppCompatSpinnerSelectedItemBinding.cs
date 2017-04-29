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
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    public class MvxAppCompatSpinnerSelectedItemBinding
        : MvxAndroidTargetBinding
    {
        private object _currentValue;

        private IDisposable _subscription;

        public MvxAppCompatSpinnerSelectedItemBinding(MvxAppCompatSpinner spinner)
            : base(spinner)
        {
        }

        protected MvxAppCompatSpinner Spinner => (MvxAppCompatSpinner) Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override Type TargetType => typeof(object);

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            var newValue = spinner.Adapter.GetRawItem(e.Position);

            bool changed;
            if (newValue == null)
                changed = _currentValue != null;
            else
                changed = !newValue.Equals(_currentValue);

            if (!changed)
                return;

            _currentValue = newValue;
            FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object value)
        {
            var spinner = (MvxAppCompatSpinner) target;

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

        public override void SubscribeToEvents()
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            _subscription = spinner.WeakSubscribe<MvxAppCompatSpinner, AdapterView.ItemSelectedEventArgs>(
                nameof(spinner.ItemSelected),
                SpinnerItemSelected);
        }

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
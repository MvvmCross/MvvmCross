// MvxSpinnerSelectedItemBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.WeakSubscription;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxSpinnerSelectedItemBinding
        : MvxConvertingTargetBinding
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
                changed = (_currentValue != null);
            }
            else
            {
                changed = !(newValue.Equals(_currentValue));
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

            _subscription = spinner.WeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
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
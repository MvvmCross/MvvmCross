// MvxSpinnerSelectedItemBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Widget;

    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Platform.Platform;

    public class MvxSpinnerSelectedItemBinding
        : MvxAndroidTargetBinding
    {
        protected MvxSpinner Spinner => (MvxSpinner)Target;

        private object _currentValue;
        private bool _subscribed;

        public MvxSpinnerSelectedItemBinding(MvxSpinner spinner)
            : base(spinner)
        {
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = this.Spinner;
            if (spinner == null)
                return;

            var newValue = spinner.Adapter.GetRawItem(e.Position);

            bool changed;
            if (newValue == null)
            {
                changed = (this._currentValue != null);
            }
            else
            {
                changed = !(newValue.Equals(this._currentValue));
            }

            if (!changed)
            {
                return;
            }

            this._currentValue = newValue;
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

            if (!value.Equals(this._currentValue))
            {
                var index = spinner.Adapter.GetPosition(value);
                if (index < 0)
                {
                    MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                    return;
                }
                this._currentValue = value;
                spinner.SetSelection(index);
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var spinner = this.Spinner;
            if (spinner == null)
                return;

            spinner.ItemSelected += this.SpinnerItemSelected;
            this._subscribed = true;
        }

        public override Type TargetType => typeof(object);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var spinner = this.Spinner;
                if (spinner != null && this._subscribed)
                {
                    spinner.ItemSelected -= this.SpinnerItemSelected;
                    this._subscribed = false;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
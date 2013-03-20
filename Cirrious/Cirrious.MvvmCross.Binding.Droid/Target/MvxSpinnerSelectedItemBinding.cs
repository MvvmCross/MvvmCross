// MvxSpinnerSelectedItemBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
    public class MvxSpinnerSelectedItemBinding : MvxAndroidTargetBinding
    {
        protected MvxSpinner Spinner
        {
            get { return (MvxSpinner) Target; }
        }

        private object _currentValue;

        public MvxSpinnerSelectedItemBinding(MvxSpinner spinner)
            : base(spinner)
        {
            spinner.ItemSelected += SpinnerItemSelected;
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            var newValue = spinner.Adapter.GetRawItem(e.Position);

            if (!newValue.Equals(_currentValue))
            {
                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        public override void SetValue(object value)
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            if (value != null && !value.Equals(_currentValue))
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

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof (object); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var spinner = Spinner;
                if (spinner != null)
                {
                    spinner.ItemSelected -= SpinnerItemSelected;
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
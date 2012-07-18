using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxSpinnerSelectedItemBinding : MvxBaseAndroidTargetBinding
    {
        private readonly MvxBindableSpinner _spinner;
        private object _currentValue;

        public MvxSpinnerSelectedItemBinding(MvxBindableSpinner spinner)
        {
            _spinner = spinner;
            _spinner.ItemSelected += _spinner_ItemSelected;
        }

        void _spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var container = (_spinner.SelectedItem as MvxJavaContainer);
            if (container == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Missing MvxJavaContainer in MvxSpinnerSelectedItemBinding");
                return;
            }
            var newValue = container.Object;
            if (!newValue.Equals(_currentValue))
            {
                FireValueChanged(newValue);
            }
        }

        public override void SetValue(object value)
        {
            if (!value.Equals(_currentValue))
            {
                _currentValue = value;
                _spinner.SetSelection(_spinner.Adapter.GetPosition(value));
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override Type TargetType
        {
            get { return typeof(object); }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _spinner.ItemSelected -= _spinner_ItemSelected;
            }
            base.Dispose(isDisposing);
        }
    }
}
using System;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Views;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Android.Target
{
    public class MvxSpinnerSelectedItemBinding : MvxBaseAndroidTargetBinding
    {
        private readonly MvxBindableSpinner _spinner;
        private object _currentValue;

        public MvxSpinnerSelectedItemBinding(MvxBindableSpinner spinner)
        {
            _spinner = spinner;
            _spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(_spinner_ItemSelected);
        }

        void _spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var newValue = (_spinner.SelectedItem as MvxJavaContainer).Object;
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
    }
}
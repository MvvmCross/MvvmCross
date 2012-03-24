using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.Conference.UI.Droid.Bindings
{
    public class FavoritesButtonBinding
        : MvxBaseAndroidTargetBinding
    {
        private readonly Button _button;
        private bool _currentValue;

        public FavoritesButtonBinding(Button button)
        {
            _button = button;
            _button.Click += ButtonOnClick;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            _currentValue = !_currentValue;
            SetButtonBackground();
            FireValueChanged(_currentValue);
        }

        public override void SetValue(object value)
        {
            var boolValue = (bool)value;
            _currentValue = boolValue;
            SetButtonBackground();
        }

        private void SetButtonBackground()
        {
            if (_currentValue)
            {
                _button.SetBackgroundResource(Resource.Drawable.star_gold_selector);
            }
            else
            {
                _button.SetBackgroundResource(Resource.Drawable.star_grey_selector);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _button.Click -= ButtonOnClick;
            }
            base.Dispose(isDisposing);
        }

        public override Type TargetType
        {
            get { return typeof(bool); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }
    }
}
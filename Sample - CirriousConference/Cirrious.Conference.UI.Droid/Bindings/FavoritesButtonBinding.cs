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
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Droid.Target;

namespace Cirrious.Conference.UI.Droid.Bindings
{
    public class FavoritesButtonBinding
        : MvxAndroidTargetBinding
    {
        protected  Button Button
        {
            get { return (Button) Target; }
        }

        private bool _currentValue;

        public FavoritesButtonBinding(Button button)
            : base(button)
        {
            button.Click += ButtonOnClick;
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
            var button = Button;
            if (button == null)
                return;

            if (_currentValue)
            {
                button.SetBackgroundResource(Resource.Drawable.star_gold_selector);
            }
            else
            {
                button.SetBackgroundResource(Resource.Drawable.star_grey_selector);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var button = Button;
                button.Click -= ButtonOnClick;
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
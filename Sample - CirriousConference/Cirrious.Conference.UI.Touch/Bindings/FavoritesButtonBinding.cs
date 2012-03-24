using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch.Bindings
{
    public class FavoritesButtonBinding
        : MvxBaseTargetBinding
    {
        public static readonly UIImage YesImage = UIImage.FromFile("ConfResources/star_gold45.png");
        public static readonly UIImage NoImage = UIImage.FromFile("ConfResources/star_grey45.png");

        public static void SetButtonBackground(UIButton button, bool value)
        {
            if (value)
            {
                button.SetImage(YesImage, UIControlState.Normal);
            }
            else
            {
                button.SetImage(NoImage, UIControlState.Normal);
            }
        }

        private readonly UIButton _button;
        private bool _currentValue;

        public FavoritesButtonBinding(UIButton button)
        {
            _button = button;
            _button.TouchDown += ButtonOnClick;
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
            SetButtonBackground(_button, _currentValue);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _button.TouchDown -= ButtonOnClick;
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
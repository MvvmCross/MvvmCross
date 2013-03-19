using System;
using System.Reflection;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using MonoTouch.UIKit;

namespace Cirrious.Conference.UI.Touch.Bindings
{
    public class FavoritesButtonBinding
        : MvxTargetBinding
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

        private UIButton Button {
			get {
				return Target as UIButton;
			}
		}

        private bool _currentValue;

        public FavoritesButtonBinding(UIButton button)
        	: base(button)
		{
            button.TouchUpInside += ButtonOnClick;
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

        private void SetButtonBackground ()
		{
			var button = Button;
			if (button != null) {
				SetButtonBackground (button, _currentValue);
			}
		}

        protected override void Dispose (bool isDisposing)
		{
			if (isDisposing) {
				var button = Button;
				if (button != null) {
					button.TouchUpInside -= ButtonOnClick;
				}
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
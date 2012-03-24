using System;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.Conference.UI.Touch.Bindings
{
    public class FavoritesSessionCellBinding
        : MvxBaseTargetBinding
    {
        private readonly SessionCell2 _cell;
        private bool _currentValue;

        public FavoritesSessionCellBinding(SessionCell2 cell)
        {
            _cell = cell;

            _cell.PublicFavoritesButtonPressed += HandlePublicFavoritesButtonPressed;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _cell.PublicFavoritesButtonPressed -= HandlePublicFavoritesButtonPressed;
            }

            base.Dispose(isDisposing);
        }

        void HandlePublicFavoritesButtonPressed(object sender, EventArgs e)
        {
            _currentValue = !_currentValue;
            FireValueChanged(_currentValue);
        }

        public override Type TargetType
        {
            get { return typeof(bool); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        public override void SetValue(object value)
        {
            _currentValue = (bool)value;
            var button = _cell.PublicFavoritesButton;
            FavoritesButtonBinding.SetButtonBackground(button, _currentValue);
        }
    }
}
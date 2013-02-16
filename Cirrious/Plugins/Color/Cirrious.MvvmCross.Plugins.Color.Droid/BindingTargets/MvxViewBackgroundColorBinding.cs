using Android.Views;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxViewBackgroundColorBinding
        : MvxBaseViewColorBinding
    {
        private readonly View _view;

        public MvxViewBackgroundColorBinding(View view)
        {
            _view = view;
        }

        public override void SetValue(object value)
        {
            _view.SetBackgroundColor((Android.Graphics.Color)value);
        }
    }
}
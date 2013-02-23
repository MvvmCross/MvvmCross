using Android.Views;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxViewBackgroundColorBinding
        : MvxBaseViewColorBinding
    {
        public MvxViewBackgroundColorBinding(View view)
            : base(view)
        {
        }

        public override void SetValue(object value)
        {
            var view = TextView;
            if (view == null)
                return;
            view.SetBackgroundColor((Android.Graphics.Color)value);
        }
    }
}
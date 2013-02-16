using Android.Widget;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxTextViewTextColorBinding
        : MvxBaseViewColorBinding
    {
        private readonly TextView _textView;

        public MvxTextViewTextColorBinding(TextView textView)
        {
            _textView = textView;
        }

        public override void SetValue(object value)
        {
            _textView.SetTextColor((Android.Graphics.Color)value);
        }
    }
}
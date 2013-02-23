using Android.Widget;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxTextViewTextColorBinding
        : MvxBaseViewColorBinding
    {
        protected TextView TextView
        {
            get { return (TextView) base.Target; }
        }

        public MvxTextViewTextColorBinding(TextView textView)
            : base(textView)
        {
        }

        public override void SetValue(object value)
        {
            var textView = TextView;
            if (textView == null)
                return;
            textView.SetTextColor((Android.Graphics.Color)value);
        }
    }
}
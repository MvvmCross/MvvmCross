// MvxTextViewTextColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;

namespace Cirrious.MvvmCross.Plugins.Color.Droid.BindingTargets
{
    public class MvxTextViewTextColorBinding
        : MvxViewColorBinding
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
            textView.SetTextColor((Android.Graphics.Color) value);
        }
    }
}
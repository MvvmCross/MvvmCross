// MvxTextViewTextColorBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;

namespace MvvmCross.Plugins.Color.Droid.BindingTargets
{
    [Preserve(AllMembers = true)]
    public class MvxTextViewTextColorBinding
        : MvxViewColorBinding
    {
        public MvxTextViewTextColorBinding(TextView textView)
            : base(textView)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var textView = (TextView)target;
            textView?.SetTextColor((Android.Graphics.Color) value);
        }
    }
}
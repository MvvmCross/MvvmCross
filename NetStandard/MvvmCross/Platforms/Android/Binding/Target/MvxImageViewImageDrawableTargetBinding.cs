using System;
using Android.Graphics.Drawables;
using Android.Widget;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewImageDrawableTargetBinding : MvxAndroidTargetBinding
    {
        public MvxImageViewImageDrawableTargetBinding(ImageView target)
            : base(target)
        {
        }

        public override Type TargetType => typeof(ImageView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            var view = (ImageView)target;
            var drawable = value as Drawable;

            view.SetImageDrawable(drawable);
        }
    }
}
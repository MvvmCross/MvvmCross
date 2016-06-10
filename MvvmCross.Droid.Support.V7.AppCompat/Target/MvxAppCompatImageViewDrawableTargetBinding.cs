// MvxAppCompatImageViewDrawableTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.OS;
    using Android.Graphics;
    using Android.Graphics.Drawables;
    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Droid.Target;
    using MvvmCross.Platform.Platform;

    public class MvxAppCompatImageViewDrawableTargetBinding
        : MvxAndroidTargetBinding
    {
        protected AppCompatImageView ImageView => (AppCompatImageView)Target;

        public MvxAppCompatImageViewDrawableTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(int);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (AppCompatImageView)target;

            if (!(value is int))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value was not a valid Drawable");
                imageView.SetImageDrawable(null);
                return;
            }

            var intValue = (int)value;

            if (intValue == 0)
                imageView.SetImageDrawable(null);
            else
            {
                var context = imageView.Context;
                Drawable drawable;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    drawable = context?.Resources?.GetDrawable(intValue, context.Theme);
                else
                    drawable = context?.Resources?.GetDrawable(intValue);

                if (drawable != null)
                    imageView.SetImageDrawable(drawable);
            }
        }
    }
}
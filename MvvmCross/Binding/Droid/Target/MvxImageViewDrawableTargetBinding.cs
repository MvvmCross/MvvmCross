// MvxImageViewDrawableTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid.Target
{
    public class MvxImageViewDrawableTargetBinding
        : MvxAndroidTargetBinding
    {
        public MvxImageViewDrawableTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        protected ImageView ImageView => (ImageView) Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(int);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView) target;

            if (!(value is int))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                    "Value was not a valid Drawable");
                imageView.SetImageDrawable(null);
                return;
            }

            var intValue = (int) value;

            if (intValue == 0)
                imageView.SetImageDrawable(null);
            else
                SetImage(imageView, intValue);
        }

        protected virtual void SetImage(ImageView imageView, int id)
        {
            var context = imageView.Context;
            Drawable drawable;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                drawable = context?.Resources?.GetDrawable(id, context.Theme);
            else
                drawable = context?.Resources?.GetDrawable(id);

            if (drawable != null)
                imageView.SetImageDrawable(drawable);
        }
    }
}
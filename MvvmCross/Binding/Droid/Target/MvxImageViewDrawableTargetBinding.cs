// MvxImageViewDrawableTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
	using System;
	using Android.Graphics.Drawables;
	using Android.Widget;
	using Android.OS;

	using MvvmCross.Platform.Platform;

	public class MvxImageViewDrawableTargetBinding
        : MvxAndroidTargetBinding
    {
        protected ImageView ImageView => (ImageView)Target;

        public MvxImageViewDrawableTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(int);

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (ImageView)target;

            if (!(value is int))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                    "Value was not a valid Drawable");
                imageView.SetImageDrawable(null);
                return;
            }

            var intValue = (int)value;

            if (intValue == 0)
                imageView.SetImageDrawable(null);
            else
            {
				var appContext = AndroidGlobals.ApplicationContext;
				Drawable drawable;
				if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
					drawable = appContext.Resources.GetDrawable(intValue, appContext.Theme);
				else
					drawable = appContext.Resources.GetDrawable(intValue);

				if (drawable != null)
					imageView.SetImageDrawable(drawable);
            }
        }
    }
}
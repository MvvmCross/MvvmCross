// MvxImageViewDrawableTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Target
{
    using System;

    using Android.Graphics;
    using Android.Widget;

    using MvvmCross.Platform.Platform;

    public class MvxImageViewDrawableTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        public MvxImageViewDrawableTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType => typeof(int);

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            if (!(value is int))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                    "Value was not a valid Drawable");
                bitmap = null;
                return false;
            }

            var intValue = (int)value;

            if (intValue == 0)
                bitmap = null;
            else
            {
                var resources = this.AndroidGlobals.ApplicationContext.Resources;
                bitmap = BitmapFactory.DecodeResource(resources, intValue, new BitmapFactory.Options() { InPurgeable = true });
            }

            return true;
        }
    }
}
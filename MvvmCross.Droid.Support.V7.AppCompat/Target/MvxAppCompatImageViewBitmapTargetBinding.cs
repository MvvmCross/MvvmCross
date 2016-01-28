// MvxAppCompatImageViewBitmapTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Support.V7.AppCompat.Target
{
    using System;

    using Android.Graphics;
    using Android.Support.V7.Widget;

    using MvvmCross.Binding;
    using MvvmCross.Platform.Platform;

    public class MvxAppCompatImageViewBitmapTargetBinding
        : MvxAppCompatBaseImageViewTargetBinding
    {
        public MvxAppCompatImageViewBitmapTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetType => typeof(Bitmap);

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            if (!(value is Bitmap))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value was not a valid Bitmap");
                bitmap = null;
                return false;
            }

            bitmap = (Bitmap)value;
            return true;
        }
    }
}
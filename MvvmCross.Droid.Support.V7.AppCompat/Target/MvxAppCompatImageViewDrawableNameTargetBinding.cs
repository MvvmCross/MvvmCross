// MvxAppCompatImageViewDrawableNameTargetBinding.cs

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

    public class MvxAppCompatImageViewDrawableNameTargetBinding
        : MvxAppCompatImageViewDrawableTargetBinding
    {
        public MvxAppCompatImageViewDrawableNameTargetBinding(AppCompatImageView imageView) : base(imageView)
        {
        }

        public override Type TargetType => typeof(string);

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            if (!(value is string))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid string identifier", value);
                bitmap = null;
                return false;
            }

            var resources = this.AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", this.AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' was not a known drawable name", value);
                bitmap = null;
                return false;
            }

            return base.GetBitmap(id, out bitmap);
        }
    }
}
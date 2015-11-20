// MvxImageViewDrawableTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Android.Widget;
using Cirrious.CrossCore.Platform;
using System;

namespace Cirrious.MvvmCross.Binding.Droid.Target
{
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
                var resources = AndroidGlobals.ApplicationContext.Resources;
                bitmap = BitmapFactory.DecodeResource(resources, intValue, new BitmapFactory.Options() { InPurgeable = true });
            }

            return true;
        }
    }
}
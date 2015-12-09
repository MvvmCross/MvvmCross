// MvxImageViewBitmapTargetBinding.cs
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
    public class MvxImageViewBitmapTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        public MvxImageViewBitmapTargetBinding(ImageView imageView)
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
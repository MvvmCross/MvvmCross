// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Graphics;
using Android.Widget;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public class MvxImageViewBitmapTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        public MvxImageViewBitmapTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        public override Type TargetValueType => typeof(Bitmap);

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            if (!(value is Bitmap))
            {
                MvxBindingLog.Warning(
                                      "Value was not a valid Bitmap");
                bitmap = null;
                return false;
            }

            bitmap = (Bitmap)value;
            return true;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    public abstract class MvxBaseStreamImageViewTargetBinding
        : MvxBaseImageViewTargetBinding
    {
        protected MvxBaseStreamImageViewTargetBinding(ImageView imageView)
            : base(imageView)
        {
        }

        protected override bool GetBitmap(object value, out Bitmap bitmap)
        {
            var assetStream = GetStream(value);
            if (assetStream == null)
            {
                bitmap = null;
                return false;
            }

            var options = new BitmapFactory.Options { InPurgeable = true };
            bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
            return true;
        }

        protected override void SetImageBitmap(ImageView imageView, Bitmap bitmap)
        {
            var drawable = new BitmapDrawable(Resources.System, bitmap);
            imageView.SetImageDrawable(drawable);
        }

        protected abstract Stream GetStream(object value);
    }
}

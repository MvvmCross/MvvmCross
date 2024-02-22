// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace MvvmCross.Platforms.Android.Binding.Target;

public abstract class MvxBaseStreamImageViewTargetBinding(ImageView imageView)
    : MvxBaseImageViewTargetBinding(imageView)
{
    protected override bool GetBitmap(object? value, out Bitmap? bitmap)
    {
        using var assetStream = GetStream(value);
        if (assetStream == null)
        {
            bitmap = null;
            return false;
        }

        var options = new BitmapFactory.Options { InPurgeable = true };
        bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
        return true;
    }

    protected override void SetImageBitmap(ImageView imageView, Bitmap? bitmap)
    {
        var drawable = new BitmapDrawable(Resources.System, bitmap);
        imageView.SetImageDrawable(drawable);
    }

    protected abstract Stream? GetStream(object? value);
}

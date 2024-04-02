// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewImageTargetBinding(ImageView imageView)
    : MvxBaseImageViewTargetBinding(imageView)
{
    public override Type TargetValueType => typeof(string);

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

    private static Stream? GetStream(object? value)
    {
        if (value == null)
        {
            MvxBindingLog.Instance?.LogWarning("Null value passed to ImageView binding");
            return null;
        }

        var stringValue = value as string;
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            MvxBindingLog.Instance?.LogWarning("Empty value passed to ImageView binding");
            return null;
        }

        var drawableResourceName = GetImageAssetName(stringValue);
        var assetStream = Application.Context.Assets?.Open(drawableResourceName);

        return assetStream;
    }

    private static string GetImageAssetName(string rawImage)
    {
        return rawImage.TrimStart('/');
    }
}

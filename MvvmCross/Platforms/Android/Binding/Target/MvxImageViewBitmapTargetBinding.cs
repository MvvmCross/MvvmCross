// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Graphics;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewBitmapTargetBinding(ImageView imageView)
    : MvxBaseImageViewTargetBinding(imageView)
{
    public override Type TargetValueType => typeof(Bitmap);

    protected override bool GetBitmap(object? value, out Bitmap? bitmap)
    {
        if (value is not Bitmap valueBitmap)
        {
            MvxBindingLog.Instance?.LogWarning("Value was not a valid Bitmap: {Value}", value);
            bitmap = null;
            return false;
        }

        bitmap = valueBitmap;
        return true;
    }
}

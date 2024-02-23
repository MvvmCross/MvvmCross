// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewResourceNameTargetBinding(ImageView imageView)
    : MvxImageViewDrawableNameTargetBinding(imageView)
{
    protected override void SetImage(ImageView view, int id)
    {
        imageView.SetImageResource(id);
    }
}

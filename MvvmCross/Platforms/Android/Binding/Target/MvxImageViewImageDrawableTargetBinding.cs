// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Graphics.Drawables;
using MvvmCross.Binding;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxImageViewImageDrawableTargetBinding(ImageView target)
    : MvxAndroidTargetBinding(target)
{
    public override Type TargetValueType => typeof(ImageView);

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (ImageView)target;
        var drawable = value as Drawable;
        view.SetImageDrawable(drawable);
    }
}

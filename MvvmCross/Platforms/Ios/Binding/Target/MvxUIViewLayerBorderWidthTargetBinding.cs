// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIViewLayerBorderWidthTargetBinding(UIView target)
    : MvxConvertingTargetBinding(target)
{
    public override Type TargetValueType => typeof(float);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = target as UIView;
        if (view?.Layer == null || value == null) return;

        view.Layer.BorderWidth = (float)value;
    }
}

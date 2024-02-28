// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.UI;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIViewVisibilityTargetBinding(UIView target)
    : MvxConvertingTargetBinding(target)
{
    protected UIView? View => (UIView?)Target;

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(MvxVisibility);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (UIView)target;
        if (value is not MvxVisibility visibility)
        {
            MvxBindingLog.Instance?.LogWarning("Visibility out of range {Value}", value);
            return;
        }

        view.Hidden = visibility switch
        {
            MvxVisibility.Visible => false,
            MvxVisibility.Collapsed => true,
            _ => view.Hidden
        };
    }
}

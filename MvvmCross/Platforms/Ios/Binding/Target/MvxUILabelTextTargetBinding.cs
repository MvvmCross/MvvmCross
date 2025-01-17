// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUILabelTextTargetBinding(UILabel target)
    : MvxConvertingTargetBinding(target)
{
    protected UILabel? View => Target as UILabel;

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(string);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (UILabel?)target;
        if (view == null)
            return;

        view.Text = (string?)value;
    }
}

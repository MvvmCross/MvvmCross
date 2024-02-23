// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public abstract class MvxBaseUIViewVisibleTargetBinding(UIView target)
    : MvxConvertingTargetBinding(target)
{
    protected UIView? View => (UIView?)Target;

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    public override Type TargetValueType => typeof(bool);
}

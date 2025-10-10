// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxToolbarSubtitleBinding(Toolbar toolbar)
    : MvxConvertingTargetBinding(toolbar)
{
    protected Toolbar? Toolbar => Target as Toolbar;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public override Type TargetValueType => typeof(string);

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is Toolbar view)
            view.Subtitle = (string?)value;
    }
}

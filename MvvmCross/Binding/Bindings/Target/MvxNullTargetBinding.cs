// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
namespace MvvmCross.Binding.Bindings.Target;

public sealed class MvxNullTargetBinding() : MvxTargetBinding(null)
{
    public override MvxBindingMode DefaultMode => MvxBindingMode.OneTime;

    public override Type TargetValueType => typeof(object);

    public override void SetValue(object? value)
    {
        // ignored
    }
}

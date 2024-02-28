// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUISwitchOnTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UISwitch target)
    : MvxTargetBinding<UISwitch, bool>(target)
{
    private MvxWeakEventSubscription<UISwitch>? _subscription;

    protected override void SetValue(bool value)
    {
        Target?.SetState(value, true);
    }

    public override void SubscribeToEvents()
    {
        var uiSwitch = Target;
        if (uiSwitch == null)
        {
            MvxBindingLog.Instance?.LogError("Switch is null in MvxUISwitchOnTargetBinding");
            return;
        }

        _subscription = uiSwitch.WeakSubscribe(nameof(uiSwitch.ValueChanged), HandleValueChanged);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing) return;

        _subscription?.Dispose();
        _subscription = null;
    }

    private void HandleValueChanged(object? sender, EventArgs e)
    {
        FireValueChanged(Target?.On ?? false);
    }
}

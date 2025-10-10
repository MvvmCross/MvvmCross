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

public class MvxUISwitchOnTargetBinding(UISwitch target)
    : MvxTargetBinding<UISwitch, bool>(target)
{
    private MvxWeakEventSubscription<UISwitch>? _subscription;

    [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
    protected override void SetValue(bool value)
    {
        Target?.SetState(value, true);
    }

    [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
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

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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

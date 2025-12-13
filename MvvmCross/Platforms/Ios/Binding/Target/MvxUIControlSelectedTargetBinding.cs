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

public class MvxUIControlSelectedTargetBinding(UIControl target)
	: MvxTargetBinding<UIControl, bool>(target)
{
    private MvxWeakEventSubscription<UIControl>? _subscription;

    [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
    protected override void SetValue(bool value)
    {
        var control = Target;
        if (control == null)
            return;

        control.Selected = value;
    }

    [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
    public override void SubscribeToEvents()
    {
        var control = Target;
        if (control == null)
        {
            MvxBindingLog.Instance?.LogError("Control is null in MvxUIControlSelectedTargetBinding");
            return;
        }

        _subscription = control.WeakSubscribe(nameof(control.TouchUpInside), HandleSelectedChanged);
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

    private void HandleSelectedChanged(object? sender, EventArgs e)
    {
        var control = Target;
        if (control == null)
            return;

        control.Selected = !control.Selected;
        FireValueChanged(control.Selected);
    }
}


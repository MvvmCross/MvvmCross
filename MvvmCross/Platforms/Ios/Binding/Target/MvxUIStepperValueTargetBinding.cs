// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUIStepperValueTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UIStepper target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UIStepper>(target, targetPropertyInfo)
{
    private MvxWeakEventSubscription<UIStepper>? _subscription;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is not UIStepper view || value == null)
            return;

        view.Value = (double)value;
    }

    private void HandleValueChanged(object? sender, EventArgs e)
    {
        var view = View;
        if (view == null) return;

        FireValueChanged(view.Value);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var stepper = View;
        if (stepper == null)
        {
            MvxBindingLog.Error("UIStepper is null in MvxUIStepperValueTargetBinding");
            return;
        }

        _subscription = stepper.WeakSubscribe(nameof(stepper.ValueChanged), HandleValueChanged);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing) return;

        _subscription?.Dispose();
        _subscription = null;
    }
}

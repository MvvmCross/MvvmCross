// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUISliderValueTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UISlider target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UISlider>(target, targetPropertyInfo)
{
    private IDisposable? _subscription;

    protected override void SetValueImpl(object target, object? value)
    {
        if (target is not UISlider view || value == null)
            return;

        view.Value = (float)value;
    }

    private void HandleSliderValueChanged(object? sender, EventArgs e)
    {
        var view = View;
        if (view == null) return;

        FireValueChanged(view.Value);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var slider = View;
        if (slider == null)
        {
            MvxBindingLog.Instance?.LogError("UISlider is null in MvxUISliderValueTargetBinding");
            return;
        }

        _subscription = slider.WeakSubscribe(nameof(slider.ValueChanged), HandleSliderValueChanged);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing)
            return;

        _subscription?.Dispose();
        _subscription = null;
    }
}

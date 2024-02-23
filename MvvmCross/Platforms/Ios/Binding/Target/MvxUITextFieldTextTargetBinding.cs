// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Extensions;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUITextFieldTextTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UITextField target)
    : MvxConvertingTargetBinding(target), IMvxEditableTextView
{
    private MvxWeakEventSubscription<UITextField>? _subscriptionChanged;
    private MvxWeakEventSubscription<UITextField>? _subscriptionEndEditing;

    protected UITextField? View => Target as UITextField;

    private void HandleEditTextValueChanged(object? sender, EventArgs e)
    {
        var view = View;
        if (view == null) return;

        FireValueChanged(view.Text);
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var view = View;
        if (view == null)
        {
            MvxBindingLog.Instance?.LogError(
                "UITextField is null in MvxUITextFieldTextTargetBinding");
            return;
        }

        _subscriptionChanged = view.WeakSubscribe(nameof(target.EditingChanged), HandleEditTextValueChanged);
        _subscriptionEndEditing = view.WeakSubscribe(nameof(target.EditingDidEnd), HandleEditTextValueChanged);
    }

    public override Type TargetValueType => typeof(string);

    protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
        => this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);

    protected override void SetValueImpl(object target, object? value)
    {
        var view = (UITextField?)target;
        if (view == null) return;

        view.Text = (string?)value;
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing) return;

        _subscriptionChanged?.Dispose();
        _subscriptionChanged = null;

        _subscriptionEndEditing?.Dispose();
        _subscriptionEndEditing = null;
    }

    public string? CurrentText
    {
        get
        {
            var view = View;
            return view?.Text;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Platforms.Ios.Binding.Target;

public class MvxUITextFieldTextFocusTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UITextField target)
    : MvxTargetBinding(target)
{
    private MvxWeakEventSubscription<UITextField>? _subscription;

    protected UITextField? TextField => Target as UITextField;

    public override Type TargetValueType => typeof(string);

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SetValue(object? value)
    {
        if (TextField == null) return;

        value ??= string.Empty;
        TextField.Text = value.ToString();
    }

    public override void SubscribeToEvents()
    {
        var textField = TextField;
        if (textField == null)
            return;

        _subscription = textField.WeakSubscribe(nameof(textField.EditingDidEnd), HandleLostFocus);
    }

    private void HandleLostFocus(object? sender, EventArgs e)
    {
        var textField = TextField;
        if (textField == null) return;

        FireValueChanged(textField.Text);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        if (!isDisposing) return;

        _subscription?.Dispose();
        _subscription = null;
    }
}

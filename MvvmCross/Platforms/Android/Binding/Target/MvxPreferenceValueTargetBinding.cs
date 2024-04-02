// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using AndroidX.Preference;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxPreferenceValueTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        Preference preference)
    : MvxAndroidTargetBinding(preference)
{
    private MvxAndroidTargetEventSubscription<Preference, Preference.PreferenceChangeEventArgs>? _subscription;

    public Preference? Preference => Target as Preference;

    public override Type TargetValueType => typeof(Preference);

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        _subscription = Preference?.WeakSubscribe<Preference, Preference.PreferenceChangeEventArgs>(
            nameof(Preference.PreferenceChange),
            HandlePreferenceChange);
    }

    protected void HandlePreferenceChange(object? sender, Preference.PreferenceChangeEventArgs e)
    {
        if (e.Preference != Preference)
            return;

        FireValueChanged(e.NewValue);
        e.Handled = true;
    }

    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        base.Dispose(isDisposing);
    }

    protected override void SetValueImpl(object target, object? value)
    {
        MvxBindingLog.Instance?.LogWarning("SetValueImpl called on generic Preference target");
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Binding;
using MvvmCross.Platforms.Android.WeakSubscription;

namespace MvvmCross.Platforms.Android.Binding.Target;

public class MvxCompoundButtonCheckedTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        object target,
        PropertyInfo targetPropertyInfo)
    : MvxAndroidPropertyInfoTargetBinding<CompoundButton>(target, targetPropertyInfo)
{
    private MvxAndroidTargetEventSubscription<CompoundButton, CompoundButton.CheckedChangeEventArgs>? _subscription;

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var compoundButton = View;
        if (compoundButton == null)
        {
            MvxBindingLog.Instance?.LogError(
                "compoundButton is null in MvxCompoundButtonCheckedTargetBinding");
            return;
        }

        _subscription = compoundButton.WeakSubscribe<CompoundButton, CompoundButton.CheckedChangeEventArgs>(
            nameof(compoundButton.CheckedChange),
            CompoundButtonOnCheckedChange);
    }

    private void CompoundButtonOnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs args)
    {
        FireValueChanged(View?.Checked);
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
}

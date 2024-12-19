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

public abstract class MvxBaseUIDatePickerTargetBinding(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
        UIDatePicker target,
        PropertyInfo targetPropertyInfo)
    : MvxPropertyInfoTargetBinding<UIDatePicker>(target, targetPropertyInfo)
{
    private readonly NSTimeZone _systemTimeZone = NSTimeZone.SystemTimeZone;
    private MvxWeakEventSubscription<UIDatePicker>? _subscription;

    private void DatePickerOnValueChanged(object? sender, EventArgs args)
    {
        var view = View;
        if (view == null) return;

        FireValueChanged(GetValueFrom(view));
    }

    protected abstract object GetValueFrom(UIDatePicker view);

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var datePicker = View;
        if (datePicker == null)
        {
            MvxBindingLog.Instance?.LogError("UIDatePicker is null in {TargetBindingType}",
                nameof(MvxBaseUIDatePickerTargetBinding));
        }
        // Only listen for value changes if we are binding against one of the value-derived properties.
        else if (TargetPropertyInfo.Name is nameof(UIDatePicker.Date) or nameof(UIDatePicker.CountDownDuration))
        {
            _subscription = datePicker.WeakSubscribe(nameof(datePicker.ValueChanged), DatePickerOnValueChanged);
        }
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        if (!isDisposing) return;

        _subscription?.Dispose();
        _subscription = null;
    }

    protected DateTime ToLocalTime(DateTime utc)
    {
        if (utc.Kind == DateTimeKind.Local)
            return utc;

        var local = utc.AddSeconds(_systemTimeZone.SecondsFromGMT(utc.ToNSDate())).WithKind(DateTimeKind.Local);

        return local;
    }

    protected DateTime ToUtcTime(DateTime local)
    {
        if (local.Kind == DateTimeKind.Utc)
            return local;

        var utc = local.AddSeconds(-_systemTimeZone.SecondsFromGMT(local.ToNSDate())).WithKind(DateTimeKind.Utc);

        return utc;
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.WeakSubscription;

namespace MvvmCross.Binding.Bindings.Target;

public class MvxWithEventPropertyInfoTargetBinding
    : MvxPropertyInfoTargetBinding
{
    private IDisposable? _subscription;

    public MvxWithEventPropertyInfoTargetBinding(
            object target,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
            PropertyInfo targetPropertyInfo)
        : base(target, targetPropertyInfo)
    {
    }

    // Note - this is public because we use it in weak referenced situations
    public void OnValueChanged(object? sender, EventArgs eventArgs)
    {
        var target = Target;
        if (target == null)
        {
            MvxBindingLog.Instance?.LogTrace(
                "Null weak reference target seen during OnValueChanged - unusual as usually Target is the sender of the value changed. Ignoring the value changed");
            return;
        }

        var value = TargetPropertyInfo.GetGetMethod()?.Invoke(target, null);
        FireValueChanged(value);
    }

    // Note - this is public because we use it in weak referenced situations
    public void OnPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
    {
        var target = Target;
        if (target == null)
        {
            MvxBindingLog.Instance?.LogTrace(
                "Null weak reference target seen during OnPropertyChanged - unusual as usually Target is the sender of the value changed. Ignoring the value changed");
            return;
        }

        if (eventArgs.PropertyName == TargetPropertyInfo.Name)
        {
            var value = TargetPropertyInfo.GetGetMethod()?.Invoke(target, null);
            FireValueChanged(value);
        }
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

    public override void SubscribeToEvents()
    {
        var target = Target;
        if (target == null)
            return;

        var viewType = target.GetType();

        // Try a specific [PropertyName]Changed event first.
        var eventInfo = GetNamedPropertyChangedEvent(viewType, TargetPropertyInfo.Name);
        if (eventInfo != null)
            _subscription = eventInfo.WeakSubscribe(target, OnValueChanged);
        else if (target is INotifyPropertyChanged notifyPropertyChangedTarget)
        {
            // If target implements INPC then try a generic PropertyChanged event next.
            eventInfo = GetPropertyChangedEvent(viewType);
            if (eventInfo != null)
                _subscription = notifyPropertyChangedTarget.WeakSubscribe(OnPropertyChanged);
        }

        // No suitable event found on target; this will be a one way binding.
    }

    private static EventInfo? GetNamedPropertyChangedEvent(Type viewType, string propertyName)
    {
        var eventName = propertyName + "Changed";
        var eventInfo = viewType.GetEvent(eventName);

        if (eventInfo == null)
            return null;

        if (eventInfo.EventHandlerType != typeof(EventHandler))
        {
            MvxBindingLog.Instance?.LogTrace(
                "Diagnostic - cannot two-way bind to {ViewType}/{EventName} on type {ViewTypeName} because eventHandler is type {EventHandlerTypeName)}",
                viewType,
                eventName,
                viewType.Name,
                eventInfo.EventHandlerType?.Name);
            return null;
        }

        return eventInfo;
    }

    private static EventInfo? GetPropertyChangedEvent(Type viewType)
    {
        const string eventName = "PropertyChanged";
        var eventInfo = viewType.GetEvent(eventName);

        if (eventInfo == null)
            return null;

        if (eventInfo.EventHandlerType != typeof(PropertyChangedEventHandler))
        {
            MvxBindingLog.Instance?.LogTrace(
                "Diagnostic - cannot two-way bind to {ViewType}/{EventName} on type {ViewTypeName} because eventHandler is type {EventHandlerTypeName}",
                viewType,
                eventName,
                viewType.Name,
                eventInfo.EventHandlerType?.Name);
            return null;
        }

        return eventInfo;
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Microsoft.Extensions.Logging;
using MvvmCross.Binding.Extensions;

namespace MvvmCross.Binding.Bindings.Target;

public abstract class MvxConvertingTargetBinding(object target)
    : MvxTargetBinding(target)
{
    private bool _isUpdatingSource;
    private bool _isUpdatingTarget;
    private object? _updatingSourceWith;

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected abstract void SetValueImpl(object target, object? value);

    public override void SetValue(object? value)
    {
        MvxBindingLog.Instance?.LogTrace("Receiving SetValue to {Value}", value);
        var t = Target;
        if (t == null)
        {
            MvxBindingLog.Instance?.LogWarning("Weak Target is null in {TypeName} - skipping set", GetType().Name);
            return;
        }

        if (ShouldSkipSetValueForPlatformSpecificReasons(t, value))
            return;

        if (ShouldSkipSetValueForViewSpecificReasons(t, value))
            return;

        var safeValue = MakeSafeValue(value);

        // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
        if (_isUpdatingSource)
        {
            if (safeValue == null)
            {
                if (_updatingSourceWith == null)
                    return;
            }
            else
            {
                if (safeValue.Equals(_updatingSourceWith))
                    return;
            }
        }

        try
        {
            _isUpdatingTarget = true;
            SetValueImpl(t, safeValue);
        }
        finally
        {
            _isUpdatingTarget = false;
        }
    }

    protected virtual bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
    {
        return false;
    }

    protected virtual bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object? value)
    {
        return false;
    }

    protected virtual object? MakeSafeValue(object? value)
    {
        var safeValue = TargetValueType.MakeSafeValue(value);
        return safeValue;
    }

    protected sealed override void FireValueChanged(object? newValue)
    {
        // we don't allow 'reentrant' updates of any kind from target to source
        if (_isUpdatingTarget || _isUpdatingSource)
            return;

        MvxBindingLog.Instance?.LogTrace("Firing changed to {NewValue}", newValue);
        try
        {
            _isUpdatingSource = true;
            _updatingSourceWith = newValue;

            base.FireValueChanged(newValue);
        }
        finally
        {
            _isUpdatingSource = false;
            _updatingSourceWith = null;
        }
    }
}

public abstract class MvxConvertingTargetBinding<TTarget, TValue> : MvxTargetBinding<TTarget, TValue>
    where TTarget : class
{
    private bool _isUpdatingSource;
    private bool _isUpdatingTarget;
    private TValue? _updatingSourceWith;

    protected MvxConvertingTargetBinding(TTarget target)
        : base(target)
    {
    }

    public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

    protected abstract void SetValueImpl(TTarget target, TValue? value);

    protected override void SetValue(TValue? value)
    {
        var target = Target;
        if (target == null)
        {
            MvxBindingLog.Instance?.LogWarning("Weak Target is null in {TypeName} - skipping set", GetType().Name);
            return;
        }

        if (ShouldSkipSetValueForPlatformSpecificReasons(target, value))
            return;

        if (ShouldSkipSetValueForViewSpecificReasons(target, value))
            return;

        var safeValue = MakeSafeValue(value);

        // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
        if (_isUpdatingSource && EqualityComparer<TValue>.Default.Equals(value, _updatingSourceWith))
        {
            return;
        }

        try
        {
            _isUpdatingTarget = true;
            SetValueImpl(target, safeValue);
        }
        finally
        {
            _isUpdatingTarget = false;
        }
    }

    protected virtual bool ShouldSkipSetValueForViewSpecificReasons(TTarget target, TValue? value)
    {
        return false;
    }

    protected virtual bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue? value)
    {
        return false;
    }

    protected virtual TValue? MakeSafeValue(TValue? value)
    {
        var safeValue = (TValue?)TargetValueType.MakeSafeValue(value);
        return safeValue;
    }

    protected sealed override void FireValueChanged(TValue? newValue)
    {
        // we don't allow 'reentrant' updates of any kind from target to source
        if (_isUpdatingTarget || _isUpdatingSource)
            return;

        MvxBindingLog.Instance?.LogTrace("Firing changed to {NewValue}", newValue);
        try
        {
            _isUpdatingSource = true;
            _updatingSourceWith = newValue;

            base.FireValueChanged(newValue);
        }
        finally
        {
            _isUpdatingSource = false;
            _updatingSourceWith = default;
        }
    }
}

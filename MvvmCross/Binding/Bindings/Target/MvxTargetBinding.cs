// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace MvvmCross.Binding.Bindings.Target;

public abstract class MvxTargetBinding : MvxBinding, IMvxTargetBinding
{
    public event EventHandler<MvxTargetChangedEventArgs>? ValueChanged;

    private readonly WeakReference _target;

    protected MvxTargetBinding(object? target)
    {
        _target = new WeakReference(target);
    }

    protected object? Target => _target.Target;

    public virtual void SubscribeToEvents()
    {
        // do nothing by default
    }

    protected virtual void FireValueChanged(object? newValue)
    {
        ValueChanged?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public abstract Type TargetValueType { get; }

    public abstract void SetValue(object? value);

    public abstract MvxBindingMode DefaultMode { get; }
}

public abstract class MvxTargetBinding<
        TTarget,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TValue
    > : MvxBinding, IMvxTargetBinding
    where TTarget : class
{
    public event EventHandler<MvxTargetChangedEventArgs>? ValueChanged;

    private readonly WeakReference<TTarget> _target;

    protected MvxTargetBinding(TTarget target)
    {
        _target = new WeakReference<TTarget>(target);
    }

    protected TTarget? Target
    {
        get
        {
            _target.TryGetTarget(out var target);
            return target;
        }
    }

    public virtual void SubscribeToEvents()
    {
        // do nothing by default
    }

    protected virtual void FireValueChanged(TValue? newValue)
    {
        ValueChanged?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
    }

    public abstract MvxBindingMode DefaultMode { get; }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public Type TargetValueType => typeof(TValue);

    protected abstract void SetValue(TValue? value);

    public void SetValue(object? value)
    {
        if (value != null && value is not TValue)
        {
            MvxBindingLog.Instance?.LogError(
                "Invalid value type for target binding {TypeName}: received {ValueTypeName} but expects {ExpectedTypeName}, and cast failed",
                GetType().Name, value.GetType().Name, typeof(TValue).Name);
            return;
        }

        SetValue(value == null ? default : (TValue)value);
    }
}

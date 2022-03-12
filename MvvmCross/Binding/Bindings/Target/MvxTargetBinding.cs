// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxTargetBinding : MvxBinding, IMvxTargetBinding
    {
        private readonly WeakReference _target;

        protected MvxTargetBinding(object target)
        {
            _target = new WeakReference(target);
        }

        protected object Target => _target.Target;

        public virtual void SubscribeToEvents()
        {
            // do nothing by default
        }

        protected virtual void FireValueChanged(object newValue)
        {
            ValueChanged?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
        }

        public abstract Type TargetValueType { get; }

        public abstract void SetValue(object value);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;

        public abstract MvxBindingMode DefaultMode { get; }
    }

    public abstract class MvxTargetBinding<TTarget, TValue> : MvxBinding, IMvxTargetBinding
        where TTarget : class
    {
        private readonly WeakReference<TTarget> _target;

        protected MvxTargetBinding(TTarget target)
        {
            _target = new WeakReference<TTarget>(target);
        }

        protected TTarget Target
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

        protected virtual void FireValueChanged(TValue newValue)
        {
            ValueChanged?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
        }

        public abstract MvxBindingMode DefaultMode { get; }

        public Type TargetValueType => typeof(TValue);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;

        protected abstract void SetValue(TValue value);

        public void SetValue(object value)
        {
            if (value != null && !(value is TValue))
                MvxBindingLog.Error($"Invalid value type for target binding {GetType().Name}: received {value.GetType().Name} but expects {typeof(TValue).Name}. And cast failed.");
            SetValue(value == null ? default(TValue) : (TValue)value);
        }
    }
}

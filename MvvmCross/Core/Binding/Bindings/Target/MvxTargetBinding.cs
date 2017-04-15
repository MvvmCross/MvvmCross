// MvxTargetBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Bindings.Target
{
    using System;

    public abstract class MvxTargetBinding : MvxBinding, IMvxTargetBinding
    {
        private readonly WeakReference _target;

        protected MvxTargetBinding(object target)
        {
            this._target = new WeakReference(target);
        }

        protected object Target => this._target.Target;

        public virtual void SubscribeToEvents()
        {
            // do nothing by default
        }

        protected virtual void FireValueChanged(object newValue)
        {
            ValueChanged?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
        }

        public abstract Type TargetType { get; }

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
                TTarget target = null;
                _target.TryGetTarget(out target);

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

        public Type TargetType => typeof(TTarget);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;

        protected abstract void SetValue(TValue value);

        public void SetValue(object value)
        {
            this.SetValue((TValue)value);
        }
    }
}
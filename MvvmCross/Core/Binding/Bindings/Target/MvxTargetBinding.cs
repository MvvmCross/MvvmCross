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
            var handler = this.ValueChanged;

            handler?.Invoke(this, new MvxTargetChangedEventArgs(newValue));
        }

        public abstract Type TargetType { get; }

        public abstract void SetValue(object value);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;

        public abstract MvxBindingMode DefaultMode { get; }
    }
}
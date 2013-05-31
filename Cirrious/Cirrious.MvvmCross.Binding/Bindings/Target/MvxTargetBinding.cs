// MvxTargetBinding.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxTargetBinding : MvxBinding, IMvxTargetBinding
    {
        private readonly WeakReference _target;

        protected MvxTargetBinding(object target)
        {
            _target = new WeakReference(target);
        }

        protected object Target
        {
            get { return _target.Target; }
        }

        protected virtual void FireValueChanged(object newValue)
        {
            var handler = ValueChanged;

            if (handler != null)
                handler(this, new MvxTargetChangedEventArgs(newValue));
        }

        #region IMvxTargetBinding Members

        public abstract Type TargetType { get; }
        public abstract void SetValue(object value);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        public abstract MvxBindingMode DefaultMode { get; }

        #endregion
    }
}
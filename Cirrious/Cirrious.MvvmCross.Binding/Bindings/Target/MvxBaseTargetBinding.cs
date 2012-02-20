using System;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Bindings.Target
{
    public abstract class MvxBaseTargetBinding  : MvxBaseBinding, IMvxTargetBinding
    {
        public abstract Type TargetType { get; }
        public abstract void SetValue(object value);

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        public abstract MvxBindingMode DefaultMode { get; }

        protected virtual void FireValueChanged(object newValue)
        {
            var handler = ValueChanged;

            if (handler != null)
                handler(this, new MvxTargetChangedEventArgs(newValue));
        }
    }
}
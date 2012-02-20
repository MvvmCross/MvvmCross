using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source
{
    public class MvxSourcePropertyBindingEventArgs : EventArgs
    {
        private readonly bool _isAvailable;
        public bool IsAvailable
        {
            get { return _isAvailable; }
        }

        private readonly object _value;
        public object Value
        {
            get { return _value; }
        }

        public MvxSourcePropertyBindingEventArgs(bool isAvailable, Object value)
        {
            _isAvailable = isAvailable;
            _value = value;
        }

        public MvxSourcePropertyBindingEventArgs(IMvxSourceBinding propertySourceBinding)
        {
            _isAvailable = propertySourceBinding.TryGetValue(out _value);
        }
    }
}
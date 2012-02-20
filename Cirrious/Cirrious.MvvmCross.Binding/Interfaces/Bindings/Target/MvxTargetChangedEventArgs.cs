using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target
{
    public class MvxTargetChangedEventArgs 
        : EventArgs
    {
        public Object Value { get; private set; }

        public MvxTargetChangedEventArgs(object value)
        {
            Value = value;
        }
    }
}
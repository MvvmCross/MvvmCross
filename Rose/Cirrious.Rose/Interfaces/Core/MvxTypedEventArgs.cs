using System;

namespace Cirrious.CrossCore.Interfaces.Core
{
    public class MvxTypedEventArgs<T> : EventArgs
    {
        public MvxTypedEventArgs(T value)
        {
            Value = value;
        }
        
        public T Value { get; private set; }
    }
}
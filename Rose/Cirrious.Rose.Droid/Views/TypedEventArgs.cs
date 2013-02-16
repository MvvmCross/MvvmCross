using System;

namespace Cirrious.MvvmCross.Droid.Views
{
    public class TypedEventArgs<T> : EventArgs
    {
        public TypedEventArgs(T value)
        {
            Value = value;
        }
        
        public T Value { get; private set; }
    }
}
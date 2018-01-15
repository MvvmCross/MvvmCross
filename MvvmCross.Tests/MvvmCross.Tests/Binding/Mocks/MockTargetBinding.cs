using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings.Target;

namespace MvvmCross.Binding.Test.Mocks
{
    public class MockTargetBinding : IMvxTargetBinding
    {
        public MockTargetBinding()
        {
            TargetType = typeof(object);
        }

        public int DisposeCalled { get; set; }

        public void Dispose()
        {
            DisposeCalled++;
        }

        public int SubscribeToEventsCalled { get; set; }

        public void SubscribeToEvents()
        {
            SubscribeToEventsCalled++;
        }

        public Type TargetType { get; set; }
        public MvxBindingMode DefaultMode { get; set; }

        public List<object> Values { get; } = new List<object>();

        public void SetValue(object value)
        {
            Values.Add(value);
        }

        public void FireValueChanged(MvxTargetChangedEventArgs args)
        {
            ValueChanged?.Invoke(this, args);
        }

        public event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
    }
}
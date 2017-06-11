using System;
using System.Collections.Generic;
using MvvmCross.Binding.Bindings.Source;
using MvvmCross.Platform.Converters;

namespace MvvmCross.Binding.Test.Mocks
{
    public class MockSourceBinding : IMvxSourceBinding
    {
        public MockSourceBinding()
        {
            SourceType = typeof(object);
        }

        public int DisposeCalled { get; set; }

        public void Dispose()
        {
            DisposeCalled++;
        }

        public Type SourceType { get; set; }

        public List<object> ValuesSet { get; } = new List<object>();

        public void SetValue(object value)
        {
            ValuesSet.Add(value);
        }

        public void FireSourceChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Changed;

        public bool TryGetValueResult;
        public object TryGetValueValue;

        public object GetValue()
        {
            if (!TryGetValueResult)
                return MvxBindingConstant.UnsetValue;

            return TryGetValueValue;
        }
    }
}
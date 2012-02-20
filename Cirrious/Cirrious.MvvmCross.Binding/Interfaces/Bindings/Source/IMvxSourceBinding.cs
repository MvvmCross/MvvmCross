using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Source
{
    public interface IMvxSourceBinding : IMvxBinding 
    {
        void SetValue(object value);
        Type SourceType { get; }
        event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;
        bool TryGetValue(out object value);
    }
}
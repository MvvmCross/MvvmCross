using System;

namespace Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target
{
    public interface IMvxTargetBinding : IMvxBinding
    {
        void SetValue(object value);
        Type TargetType { get; }
        event EventHandler<MvxTargetChangedEventArgs> ValueChanged;
        MvxBindingMode DefaultMode { get; }
    }
}
using System;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Bindings.PathSource;

namespace Cirrious.MvvmCross.Binding.Binders
{
    public interface IMvxSourceStep : IMvxBinding
    {
        Type TargetType { get; set; }
        Type SourceType { get; }
        void SetValue(object value);
        event EventHandler<MvxSourcePropertyBindingEventArgs> Changed;
        bool TryGetValue(out object value);
        object DataContext { get; set; }
    }
}
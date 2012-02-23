using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingTouchView
        : IMvxServiceConsumer<IMvxBinder>
    {
        List<IMvxUpdateableBinding> Bindings { get; }
        object DefaultBindingSource { get; }
    }
}
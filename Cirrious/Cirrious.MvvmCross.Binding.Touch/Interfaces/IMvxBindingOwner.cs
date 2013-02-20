using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingOwner 
        : IMvxDataConsumer
    {
        List<IMvxUpdateableBinding> Bindings { get; }
    }
}
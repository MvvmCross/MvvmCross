using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.MvvmCross.Binding.Interfaces;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingOwner 
        : IMvxDataConsumer
    {
#warning Unify this with the Android project - shouldbe easy? Well, should be possible...
        List<IMvxUpdateableBinding> Bindings { get; }
    }
}
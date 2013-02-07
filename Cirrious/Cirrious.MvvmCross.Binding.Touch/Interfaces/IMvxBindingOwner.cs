using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Interfaces.Views;

namespace Cirrious.MvvmCross.Binding.Touch.Interfaces
{
    public interface IMvxBindingOwner 
        : IDataContext
    {
        List<IMvxUpdateableBinding> Bindings { get; }
    }
}
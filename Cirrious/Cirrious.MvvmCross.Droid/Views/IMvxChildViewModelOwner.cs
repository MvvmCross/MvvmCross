using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxChildViewModelOwner 
        : IMvxServiceConsumer
    {
        List<int> OwnedSubViewModelIndicies { get;  } 
    }
}
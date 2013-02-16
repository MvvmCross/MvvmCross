using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Droid.Views
{
    public interface IMvxChildViewModelOwner 
        : IMvxServiceConsumer
    {
        List<int> OwnedSubViewModelIndicies { get;  } 
    }
}
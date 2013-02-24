using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
        , IMvxConsumer
    {
        protected IDataStore DataStore
        {
            get { return this.Resolve<IDataStore>(); }
        }
    }
}
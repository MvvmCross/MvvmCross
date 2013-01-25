using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
        , IMvxServiceConsumer
    {
        protected IDataStore DataStore
        {
            get { return this.GetService<IDataStore>(); }
        }
    }
}
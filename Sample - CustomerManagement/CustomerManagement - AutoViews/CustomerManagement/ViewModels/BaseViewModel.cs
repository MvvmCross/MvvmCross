using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces.Models;

namespace CustomerManagement.AutoViews.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel
        , IMvxConsumer
    {
        protected IDataStore DataStore
        {
            get { return this.GetService<IDataStore>(); }
        }
    }
}
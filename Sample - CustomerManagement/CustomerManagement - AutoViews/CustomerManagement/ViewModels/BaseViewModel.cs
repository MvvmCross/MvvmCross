using Cirrious.CrossCore.Interfaces.IoC;
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
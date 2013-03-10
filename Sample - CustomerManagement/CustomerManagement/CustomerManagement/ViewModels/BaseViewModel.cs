using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core.Interfaces;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class BaseViewModel 
        : MvxViewModel        
    {
        protected void RequestClose()
        {
            var closer = Mvx.Resolve<IViewModelCloser>();
            closer.RequestClose(this);
        }

        protected IDataStore DataStore
        {
            get { return Mvx.Resolve<IDataStore>(); }
        }
    }
}
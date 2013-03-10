using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces.Models;
using CustomerManagement.Core.Interfaces;

namespace CustomerManagement.AutoViews.Core.ViewModels
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
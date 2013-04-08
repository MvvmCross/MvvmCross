using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces;
using CustomerManagement.AutoViews.Core.Interfaces.Models;

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
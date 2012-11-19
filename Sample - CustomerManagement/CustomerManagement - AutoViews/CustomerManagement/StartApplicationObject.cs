using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Core
{
    public class StartApplicationObject 
        : MvxApplicationObject
        , IMvxStartNavigation
    {
        public void Start()
        {
            this.RequestNavigate<CustomerListViewModel>();
        }

        public bool ApplicationCanOpenBookmarks
        {
            get { return false; }
        }
    }
}
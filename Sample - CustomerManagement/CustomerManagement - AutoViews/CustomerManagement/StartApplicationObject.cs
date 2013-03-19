using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.ViewModels;

namespace CustomerManagement.AutoViews.Core
{
    public class StartApplicationObject 
        : MvxNavigatingObject
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
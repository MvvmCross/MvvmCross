using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.ViewModels;

namespace CustomerManagement
{
    public class StartApplicationObject : MvxApplicationObject
    {
        public void Start()
        {
            this.RequestNavigate<CustomerListViewModel>();
        }
    }
}
using Cirrious.MvvmCross.Interfaces;

namespace CustomerManagement.ViewModels
{
    public class StartViewModel : BaseNavigatingViewModel
    {
        public void Start()
        {
            this.RequestNavigate<CustomerListViewModel>();
        }
    }
}
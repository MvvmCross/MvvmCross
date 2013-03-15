using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Descriptions;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class CustomerListViewModel
        : BaseViewModel
    {
        public IObservableCollection<Customer> Customers { get { return DataStore.Customers; } }

        public ICommand CustomerSelectedCommand
        {
            get
            {
                return new MvxRelayCommand<Customer>(DoCustomerSelect);
            }
        }

        public void DoCustomerSelect(Customer customer)
        {
            RequestNavigate<DetailsCustomerViewModel>(new { customerId = customer.ID });
        }

        public ICommand AddCommand
        {
            get
            {
                return new MvxRelayCommand(DoAdd);
            }
        }

        public void DoAdd()
        {
            RequestNavigate<NewCustomerViewModel>();
        }
    }
}

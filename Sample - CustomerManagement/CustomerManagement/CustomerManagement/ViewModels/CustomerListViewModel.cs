using System.Windows.Input;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class CustomerListViewModel : BaseViewModel 
    {
        public CustomerListViewModel()
        {
            
        }

        public IObservableCollection<Customer> Customers { get { return DataStore.Customers; } }

        public ICommand CustomerSelectedCommand
        {
            get
            {
                return new MvxRelayCommand<Customer>((customer) => RequestNavigate<DetailsCustomerViewModel>(new { customerId = customer.ID }));
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new MvxRelayCommand(OnAdd);
            }
        }

        private void OnAdd()
        {
            RequestNavigate<NewCustomerViewModel>();
        }
    }
}

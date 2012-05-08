using System.Windows.Input;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public abstract class BaseEditCustomerViewModel
        : BaseViewModel
    {
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            private set { _customer = value; RaisePropertyChanged("Customer"); }
        }

        protected BaseEditCustomerViewModel(string customerId)
        {
            Customer = new Customer();
            if (customerId != null)
            {
                var original = DataStore.GetCustomer(customerId);
                Customer.CloneFrom(original);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new MvxRelayCommand(DoSave);
            }
        }

        public abstract void DoSave();

        protected void UpdateCustomer()
        {
            DataStore.UpdateCustomer(Customer);
        }

        protected void AddNewCustomer()
        {
            DataStore.CreateCustomer(Customer);
        }
    }
}
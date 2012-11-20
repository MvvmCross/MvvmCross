using CustomerManagement.AutoViews.Core.Models;

namespace CustomerManagement.AutoViews.Core.Interfaces.Models
{
    public interface IDataStore
    {
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(string customerId);
        void CreateCustomer(Customer customer);
        Customer GetCustomer(string customerId);
        IObservableCollection<Customer> Customers { get; }
    }
}
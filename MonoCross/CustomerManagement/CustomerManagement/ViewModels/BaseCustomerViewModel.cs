using CustomerManagement.Shared.Model;

namespace CustomerManagement.ViewModels
{
    public class BaseCustomerViewModel : BaseNavigatingViewModel
    {
        public Customer Customer { get; set; }
    }
}
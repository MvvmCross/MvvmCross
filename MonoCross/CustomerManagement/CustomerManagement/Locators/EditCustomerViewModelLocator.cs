using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class EditCustomerViewModelLocator : BaseCustomerViewModelLocator<EditCustomerViewModel>
    {
        public EditCustomerViewModel New()
        {
            var model = new Customer();
            var viewModel = new EditCustomerViewModel() {Customer = model};
            return viewModel;
        }

        public EditCustomerViewModel Edit(string customerId)
        {
            var model = GetCustomer(customerId);
            var viewModel = new EditCustomerViewModel() { Customer = model };
            return viewModel;
        }

    }
}
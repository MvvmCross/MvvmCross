using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class DetailsCustomerViewModelLocator : BaseCustomerViewModelLocator<DetailsCustomerViewModel>
    {
        public DetailsCustomerViewModelLocator()
        {            
        }

        public DetailsCustomerViewModel Details(string customerId)
        {
            var model = GetCustomer(customerId);
            var viewModel = new DetailsCustomerViewModel() {Customer = model};
            return viewModel;
        }
    }
}

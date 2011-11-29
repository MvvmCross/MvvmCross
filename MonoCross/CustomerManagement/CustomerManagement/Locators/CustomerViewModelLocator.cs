using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class CustomerViewModelLocator : MvxViewModelLocator
    {
        public DetailsCustomerViewModel Details(string customerId)
        {
            var model = GetCustomer(customerId);
            var viewModel = new DetailsCustomerViewModel() {Customer = model};
            return viewModel;
        }

        public NewCustomerViewModel New()
        {
            var model = new Customer();
            var viewModel = new NewCustomerViewModel() { Customer = model };
            return viewModel;
        }

        public EditCustomerViewModel Edit(string customerId)
        {
            var model = GetCustomer(customerId);
            var viewModel = new EditCustomerViewModel() { Customer = model };
            return viewModel;
        }


        private static Customer GetCustomer(string customerId)
        {
            var toReturn = GetCustomerImpl(customerId);
            if (toReturn == null)
                throw new MvxException("Customer not found " + customerId);
            return toReturn;
        }

        private static Customer GetCustomerImpl(string customerId)
        {
#if LOCAL_DATA
            return CustomerManagement.Data.XmlDataStore.GetCustomer(customerId);
#else
            string urlCustomers = string.Format("http://localhost/MvxDemo/customers/{0}.xml", customerId);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            XmlSerializer serializer = new XmlSerializer(typeof(Customer));
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true))
            {
                return (Customer)serializer.Deserialize(reader);
            }
#endif
        }
    }
}

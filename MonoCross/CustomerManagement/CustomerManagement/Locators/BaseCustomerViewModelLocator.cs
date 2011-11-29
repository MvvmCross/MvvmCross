using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class BaseCustomerViewModelLocator<TViewModel> : MvxViewModelLocator where TViewModel : BaseCustomerViewModel 
    {
        public BaseCustomerViewModelLocator()
            :base()
        {            
        }

        protected static Customer GetCustomer(string customerId)
        {
            var toReturn = GetCustomerImpl(customerId);
            if (toReturn == null)
                throw new MvxException("Customer not found " + customerId);
            return toReturn;
        }

        protected static Customer GetCustomerImpl(string customerId)
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
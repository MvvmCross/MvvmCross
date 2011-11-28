using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class BaseCustomerViewModelLocator<TViewModel> : MvxViewModelLocator<TViewModel> where TViewModel : BaseCustomerViewModel 
    {
        public BaseCustomerViewModelLocator()
            :base()
        {            
        }

        public BaseCustomerViewModelLocator(string defaultActionName)
            :base(defaultActionName)
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

#warning DeadCode
        public static bool UpdateCustomer(Customer customer)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.UpdateCustomer(customer);
#else
            string urlCustomers = "http://localhost/MvxDemo/customers/customer.xml";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            request.Method = "PUT";
            request.ContentType = "application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Customer));
                serializer.Serialize(dataStream, customer);
            }

            request.GetResponse();
#endif
            return true;
        }

        public static bool AddNewCustomer(Customer customer)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.CreateCustomer(customer);
#else
            string urlCustomers = "http://localhost/MvxDemo/customers/customer.xml";

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            request.Method = "POST";
            request.ContentType = "application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Customer));
                serializer.Serialize(dataStream, customer);
            }

            request.GetResponse();
#endif
            return true;
        }

        public static bool DeleteCustomer(string customerId)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.DeleteCustomer(customerId);
#else
        string urlCustomers = "http://localhost/MvxDemo/customers/" + customerId;

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
        request.Method = "DELETE";
        request.GetResponse();
#endif
            return true;
        }

    }
}
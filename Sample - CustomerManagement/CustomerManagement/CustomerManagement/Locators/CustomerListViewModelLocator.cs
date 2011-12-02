using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModel;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Locators
{
    public class CustomerListViewModelLocator : MvxViewModelLocator
    {
        public CustomerListViewModelLocator()
        {            
        }

        public CustomerListViewModel Index()
        {
            // populate model
            var model = GetCustomerList();
            var viewModel = new CustomerListViewModel() {Customers = model};
            return viewModel;
        }

        public static List<Customer> GetCustomerList()
        {
            List<Customer> customerList = new List<Customer>();           
#if LOCAL_DATA
            customerList = CustomerManagement.Data.XmlDataStore.GetCustomers();
#else
            // XML Serializer
            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(List<Customer>));

            // web request
            string urlCustomers = "http://localhost/MvxDemo/customers.xml";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true))
            {
                // XML serializer
                customerList = (List<Customer>)serializer.Deserialize(reader);
            }
#endif
            return customerList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

using System.Xml.Serialization;
using Cirrious.MonoCross.Extensions.Controllers;
using MonoCross.Navigation;

using CustomerManagement.Shared;
using CustomerManagement.Shared.Model;
using MonoCross.Navigation.ActionResults;

namespace CustomerManagement.Controllers
{
    public class CustomerListController : MXConventionBasedController
    {
        public IMXActionResult Index()
        {
            // populate model
            var model = GetCustomerList();
            return ShowView<List<Customer>>(ViewPerspective.Default, model);
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
            string urlCustomers = "http://localhost/MXDemo/customers.xml";
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

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
using MonoCross.Navigation.Exceptions;

namespace CustomerManagement.Controllers
{
    public class CustomerController : MXConventionBasedController<Customer>
    {
        public CustomerController()
            : base("GET")
        {            
        }

        public IMXActionResult GET(string customerId)
        {
            Model = GetCustomer(customerId);
            return ShowView(ViewPerspective.Default);
        }

        public IMXActionResult NEW()
        {
            Model = new Customer();
            return ShowView(ViewPerspective.Update);
        }

        public IMXActionResult EDIT(string customerId)
        {
            Model = GetCustomer(customerId);
            return ShowView(ViewPerspective.Update);
        }

        public IMXActionResult DELETE(string customerId)
        {
            DeleteCustomer(customerId);
            return RedirectToUrl("Customers");
        }

        public IMXActionResult CREATE()
        {
            // process addition of new model
            if (!AddNewCustomer(Model))
                return ShowError("Customer not added - sorry");

            return RedirectToUrl("Customers");
        }

        public IMXActionResult UPDATE()
        {
            // process addition of new model
            if (!UpdateCustomer(Model))
                return ShowError("Customer not updated - sorry");

            return RedirectToUrl("Customers");
        }

        private static Customer GetCustomer(string customerId)
        {
            var toReturn = GetCustomerImpl(customerId);
            if (toReturn == null)
                throw new MonoCrossException("Customer not found " + customerId);
            return toReturn;
        }

        private static Customer GetCustomerImpl(string customerId)
        {
#if LOCAL_DATA
            return CustomerManagement.Data.XmlDataStore.GetCustomer(customerId);
#else
            string urlCustomers = string.Format("http://localhost/MXDemo/customers/{0}.xml", customerId);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
            XmlSerializer serializer = new XmlSerializer(typeof(Customer));
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), true))
            {
                return (Customer)serializer.Deserialize(reader);
            }
#endif
        }
		
		public static bool UpdateCustomer(Customer customer)
        {
#if LOCAL_DATA
            CustomerManagement.Data.XmlDataStore.UpdateCustomer(customer);
#else
            string urlCustomers = "http://localhost/MXDemo/customers/customer.xml";

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
            string urlCustomers = "http://localhost/MXDemo/customers/customer.xml";

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
        string urlCustomers = "http://localhost/MXDemo/customers/" + customerId;

        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlCustomers);
        request.Method = "DELETE";
        request.GetResponse();
#endif
            return true;
        }
    }
}

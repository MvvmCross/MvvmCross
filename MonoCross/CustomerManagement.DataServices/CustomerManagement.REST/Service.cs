using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using System.Web;

using CustomerManagement.Data;
using CustomerManagement.Shared.Model;

namespace CustomerManagement.REST
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Service
    {
        #region XML Services

        #region Chapter 7 Method Snippets
        // GET Methods
        //[XmlSerializerFormat]
        //[WebGet(UriTemplate = "customers.xml")]
        //public List<Customer> GetCustomers()
        //{
        //    return XmlDataStore.GetCustomers();
        //}

        //[XmlSerializerFormat]
        //[WebGet(UriTemplate = "customers.xml?show={show}")]
        //public List<Customer> GetCustomers(string show)
        //{
        //    return show == null ?
        //        XmlDataStore.GetCustomers() :
        //        XmlDataStore.GetCustomers(show.Split(char.Parse(",")));
        //}

        //[XmlSerializerFormat]
        //[WebGet(UriTemplate = "customers.xml?show={show}&page={page}&size={size}")]
        //public List<Customer> GetCustomers(string show, int page, int size)
        //{
        //    return show == null ?
        //        XmlDataStore.GetCustomers(page == null ? 1 : page, size == null ? 5 : size) :
        //        XmlDataStore.GetCustomers(show.Split(char.Parse(",")), page == null ? 1 : page, size == null ? 10 : size);
        //}

        //[XmlSerializerFormat]
        //[WebGet(UriTemplate = "customers.xml?show={show}&page={page}&items={items}")]
        //public List<Customer> GetCustomers(string show, int page, int items)
        //{
        //    return show == null ?
        //        XmlDataStore.GetCustomers(page == 0 ? 1 : page, items == 0 ? 5 : items) :
        //        XmlDataStore.GetCustomers(show.Split(char.Parse(",")), page == 0 ? 1 : page, items == 0 ? 10 : items);
        //}
        #endregion

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "customers.xml?filter={filter}&show={show}&page={page}&items={items}")]
        public List<Customer> GetCustomers(string filter, string show, int page, int items)
        {            
            return show == null ?
                   page == 0 && items == 0 ?
                XmlDataStore.GetCustomers(filter) :
                XmlDataStore.GetCustomers(filter, page, items) :
                XmlDataStore.GetCustomers(filter, show.Split(char.Parse(",")), page, items);
        }

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "products.xml")]
        public List<Product> GetProducts()
        {
            return XmlDataStore.GetProducts();
        }

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "orders/{customer}.xml")]
        public List<Order> GetCompanyOrders(string customer)
        {
            return XmlDataStore.GetCustomerOrders(customer);
        }

        //[XmlSerializerFormat]
        //[WebGet(UriTemplate = "orders/{customer}/{order}.xml")]
        //public List<Order> GetCompanyOrder(string customer, string order)
        //{
        //    return XmlDataStore.GetCompanyOrder(customer, order);
        //}

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "customers/{customer}.xml")]
        public Customer GetCustomer(string customer)
        {
            return XmlDataStore.GetCustomer(customer);
        }

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "customers/{customer}/contacts.xml", ResponseFormat = WebMessageFormat.Xml)]
        public List<Contact> GetContacts(string customer)
        {
            return XmlDataStore.GetContacts(customer);
        }

        [XmlSerializerFormat]
        [WebGet(UriTemplate = "customers/{customer}/{contact}.xml")]
        public Contact GetContact(string customer, string contact)
        {
            return XmlDataStore.GetContact(customer, contact);
        }      
        
        // POST Methods
        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "customers/customer.xml", Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Customer CreateCustomer(Customer instance)
        {
            return XmlDataStore.CreateCustomer(instance);
        }
        
        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "customers/{customer}/contact.xml", Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Contact CreateContact(string customer, Contact instance)
        {
            return XmlDataStore.CreateContact(customer, instance);
        }

        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "orders.xml", Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Order CreateOrder(Order instance)
        {
            return XmlDataStore.CreateOrder(instance);
        }

        // PUT Methods
        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "customers/customer.xml", Method = "PUT", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Customer UpdateCompany(Customer instance)
        {
            return XmlDataStore.UpdateCustomer(instance);
        }

        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "customers/{customer}/contact.xml", Method = "PUT", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Contact UpdateContact(string customer, Contact instance)
        {
            return XmlDataStore.UpdateContact(customer, instance);
        }

        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "orders.xml", Method = "PUT", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        public Order UpdateOrder(Order instance)
        {
            return XmlDataStore.UpdateOrder(instance);
        }

        #endregion

        #region JSON Services

        // GET Methods
        [WebGet(UriTemplate = "customers.json?filter={filter}", ResponseFormat = WebMessageFormat.Json)]
        public List<Customer> GetCustomersJson(string filter)
        {
            return XmlDataStore.GetCustomers(filter);
        }

        [WebGet(UriTemplate = "products.json", ResponseFormat = WebMessageFormat.Json)]
        public List<Product> GetProductsJson()
        {
            return XmlDataStore.GetProducts();
        }

        [WebGet(UriTemplate = "orders/{customer}.json", ResponseFormat = WebMessageFormat.Json)]
        public List<Order> GetCompanyOrdersJson(string customer)
        {
            return XmlDataStore.GetCustomerOrders(customer);
        }

        //[WebGet(UriTemplate = "orders/{customer}/{order}.json", ResponseFormat = WebMessageFormat.Json)]
        //public List<Order> GetCompanyOrderJson(string customer, string order)
        //{
        //    return XmlDataStore.GetCompanyOrder(customer, order);
        //}

        [WebGet(UriTemplate = "customers/{customer}.json", ResponseFormat = WebMessageFormat.Json)]
        public Customer GetCompanyJson(string customer)
        {
            return XmlDataStore.GetCustomer(customer);
        }

        [WebGet(UriTemplate = "customers/{customer}/contacts.json", ResponseFormat = WebMessageFormat.Json)]
        public List<Contact> GetContactsJson(string customer)
        {
            return XmlDataStore.GetContacts(customer);
        }

        [WebGet(UriTemplate = "customers/{customer}/{contact}.json", ResponseFormat = WebMessageFormat.Json)]
        public Contact GetContactJson(string customer, string contact)
        {
            return XmlDataStore.GetContact(customer, contact);
        }

        // POST Methods
        [WebInvoke(UriTemplate = "customers/customer.json", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Customer CreateCompanyJson(Customer instance)
        {
            return XmlDataStore.CreateCustomer(instance);
        }

        [WebInvoke(UriTemplate = "customers/{customer}/contact.json", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Contact CreateContactJson(string customer, Contact instance)
        {
            return XmlDataStore.CreateContact(customer, instance);
        }

        [WebInvoke(UriTemplate = "orders.json", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Order CreateOrderJson(Order instance)
        {
            return XmlDataStore.CreateOrder(instance);
        }

        // PUT Methods
        [WebInvoke(UriTemplate = "customers/customer.json", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Customer UpdateCompanyJson(Customer instance)
        {
            return XmlDataStore.UpdateCustomer(instance);
        }

        [WebInvoke(UriTemplate = "customers/{customer}/contact.json", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Contact UpdateContactJson(string customer, Contact instance)
        {
            return XmlDataStore.UpdateContact(customer, instance);
        }

        [WebInvoke(UriTemplate = "orders.json", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public Order UpdateOrderJson(Order instance)
        {
            return XmlDataStore.UpdateOrder(instance);
        }

        #endregion

        // DELETE Methods
        [WebInvoke(UriTemplate = "customers/{customer}", Method = "DELETE")]
        public void DeleteCompany(string customer)
        {
            XmlDataStore.DeleteCustomer(customer);
        }

        //[WebInvoke(UriTemplate = "customers/{customer}/{contact}", Method = "DELETE")]
        //public void DeleteContact(string customer, string contact)
        //{
        //    XmlDataStore.DeleteContact(customer, contact);
        //}

        [WebInvoke(UriTemplate = "orders/{order}", Method = "DELETE")]
        public void DeleteOrder(string order)
        {
            XmlDataStore.DeleteOrder(order);
        }
    }
}

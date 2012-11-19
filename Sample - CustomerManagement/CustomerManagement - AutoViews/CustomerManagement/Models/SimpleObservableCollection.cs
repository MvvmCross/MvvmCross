using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomerManagement.Core.Models
{
    public class SimpleObservableCollection<T>
        : ObservableCollection<T>
        , IObservableCollection<T>
    {
        public SimpleObservableCollection(List<T> source)
            : base(source)
        {            
        }
    }


    /*
    public class XmlDataStore
    {
        public static List<Customer> GetCustomers()
        {
            return GetCustomers(string.Empty);
        }
		public static List<Customer> GetCustomers(string filter)
        {
            bool test = string.IsNullOrWhiteSpace(filter);
            return (from item in GetCustomerList()
                    where item.Name.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.Name) ||
                          item.PrimaryAddress.City.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.City) ||
                          item.PrimaryAddress.State.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.State) ||
                          item.PrimaryAddress.Zip.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.Zip)
                    select new Customer()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Website = item.Website
                }).ToList();
        }

        public static List<Customer> GetCustomers(string filter, int page, int items)
        {
            return (from item in GetCustomerList()
                    where item.Name.Contains(string.IsNullOrWhiteSpace(filter) ? filter : item.Name) ||
                          item.PrimaryAddress.City.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.City) ||
                          item.PrimaryAddress.State.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.State) ||
                          item.PrimaryAddress.Zip.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.Zip)
                    select new Customer()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Website = item.Website
                    }).Skip((page - 1) * items).Take(items).ToList();
        }

        public static List<Customer> GetCustomers(string[] show)
        {
            return (from item in GetCustomerList()
                    select new Customer()
                    {
                        ID = show.Contains("id") ? item.ID : null,
                        Name = show.Contains("name") ? item.Name : null,
                        PrimaryAddress = show.Contains("primaryaddress") ? item.PrimaryAddress : null,
                        PrimaryPhone = show.Contains("primaryphone") ? item.PrimaryPhone : null,
                        Website = show.Contains("website") ? item.Website : null,
                        Addresses = show.Contains("addresses") ? item.Addresses : null,
                        Contacts = show.Contains("contacts") ? item.Contacts : null,
                        Orders = show.Contains("orders") ? item.Orders : null,
                    }).ToList();
        }

        public static List<Customer> GetCustomers(string filter, string[] show, int page, int items)
        {
            List<Customer> retval = (from item in GetCustomerList()
                    where item.Name.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.Name) ||
                          item.PrimaryAddress.City.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.City) ||
                          item.PrimaryAddress.State.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.State) ||
                          item.PrimaryAddress.Zip.Contains(!string.IsNullOrWhiteSpace(filter) ? filter : item.PrimaryAddress.Zip)
                    select new Customer()
                    {
                        ID = show.Contains("id") ? item.ID : null,
                        Name = show.Contains("name") ? item.Name : null,
                        PrimaryAddress = show.Contains("primaryaddress") ? item.PrimaryAddress : null,
                        PrimaryPhone = show.Contains("primaryphone") ? item.PrimaryPhone : null,
                        Website = show.Contains("website") ? item.Website : null,
                        Addresses = show.Contains("addresses") ? item.Addresses : null,
                        Contacts = show.Contains("contacts") ? item.Contacts : null,
                        Orders = show.Contains("orders") ? item.Orders : null,
                    }).Skip(page - 1 * items).ToList();
            return items == 0 ? retval : retval.Take(items).ToList();
        }

        public static List<Product> GetProducts()
        {
            return GetProductList();
        }

        public static Product GetProduct(string productId)
        {
            return GetProductList().Where(obj => obj.ID == productId).FirstOrDefault();
        }

        public static List<Order> GetCustomerOrders(string customer)
        {
            return (from item in GetOrderList()
                    where item.Customer.ID == customer
                    select new Order()
                    {
                        ID = item.ID,
                        PurchaseOrder = item.PurchaseOrder,
                        Customer = item.Customer
                    }).ToList();
        }

        public static List<Order> GetCustomerOrder(string customer, string orderId)
        {
            return (from item in GetOrderList()
                    where item.Customer.ID == customer && item.ID == orderId
                    select new Order()
                    {
                        ID = item.ID,
                        PurchaseOrder = item.PurchaseOrder,
                        Customer = item.Customer,
                        Items = item.Items
                    }).ToList();
        }

        public static Customer GetCustomer(string customer)
        {
            Customer retval = GetCustomerList().Where(obj => obj.ID == customer).FirstOrDefault();
            if (retval == null)
                return retval;

            if (retval.Addresses.Count > 0 && retval.PrimaryAddress == null)
            {
                retval.PrimaryAddress = new Address()
                {
                    ID = retval.Addresses[0].ID,
                    Description = retval.Addresses[0].Description,
                    Street1 = retval.Addresses[0].Street1,
                    Street2 = retval.Addresses[0].Street2,
                    City = retval.Addresses[0].City,
                    State = retval.Addresses[0].State,
                    Zip = retval.Addresses[0].Zip
                };
            }
            return retval;
        }

        public static ObservableCollection<Contact> GetContacts(string customer)
        {
            return 
                new ObservableCollection<Contact>(GetCustomerList().Where(obj => obj.ID == customer).First().Contacts);
        }

        public static Contact GetContact(string customer, string contact)
        {
            Contact retval = GetContacts(customer).Where(obj => obj.ID == contact).FirstOrDefault();
            return retval;
        }

        // Create Methods
        public static Customer CreateCustomer(Customer instance)
        {
            var companies = GetCustomerList();

            // Set ID's
            string ID = (companies.Max(a => Convert.ToInt32(a.ID)) + 1).ToString();
            instance.ID = ID;
            instance.PrimaryAddress.ID = string.Format("{0}-a1", ID);

            companies.Add(instance);
            SaveCustomers(companies);
            return instance;
        }

        public static Contact CreateContact(string customer, Contact instance)
        {
            var contacts = GetContacts(customer);

            // Set ID
            string ID = (contacts.Count + 1).ToString();
            instance.ID = string.Format("{0}-c{1}", customer, ID);

            contacts.Add(instance);
            SaveContacts(customer, contacts);
            return instance;
        }

        public static Order CreateOrder(Order instance)
        {
            List<Order> orders = GetOrderList();

            // Set ID
            string ID = (int.Parse(orders.Max(a => a.ID)) + 1).ToString();
            instance.ID = ID;

            orders.Add(instance);
            SaveOrders(orders);
            return instance;
        }

        // Update Methods
        public static Customer UpdateCustomer(Customer instance)
        {
            List<Customer> companies = GetCustomerList();
            if (companies.Where(obj => obj.ID == instance.ID).Count() != 0)
                companies.Remove(companies.First(obj => obj.ID == instance.ID));
            companies.Add(instance);
            SaveCustomers(companies);
            return instance;
        }

        public static Contact UpdateContact(string customer, Contact instance)
        {
            var contacts = GetContacts(customer);
            contacts.Remove(contacts.First(obj => obj.ID == instance.ID));
            contacts.Add(instance);
            SaveContacts(customer, contacts);
            return instance;
        }

        public static Order UpdateOrder(Order instance)
        {
            List<Order> orders = GetOrderList();
            orders.Remove(orders.First(obj => obj.ID == instance.ID));
            orders.Add(instance);
            SaveOrders(orders);
            return instance;
        }

        // Delete Methods
        public static void DeleteCustomer(string customer)
        {
            List<Customer> companies = GetCustomerList();
            companies.Remove(companies.First(obj => obj.ID == customer));
            SaveCustomers(companies);
        }

        //public static void DeleteContact(string customer, string contact)
        //{
        //    List<Contact> contacts = GetContacts(customer);
        //    contacts.Remove(contacts.First(obj => obj.ID == contact));
        //    SaveContacts(customer, contacts);
        //}

        public static void DeleteOrder(string order)
        {
            List<Order> orders = GetOrderList();
            orders.Remove(orders.First(obj => obj.ID == order));
            SaveOrders(orders);
        }

        private static ObservableCollection<Customer> CachedCustomerList;

		//
        // File system Access 
		//
        static ObservableCollection<Customer> GetCustomerList()
        {
            if (CachedCustomerList != null)
                return CachedCustomerList;

			string dataFilePath = Path.Combine(AppPath, Path.Combine("Xml", "Customers.xml"));

			var loadedData = XDocument.Load(dataFilePath);
	        using (var reader = loadedData.Root.CreateReader())
			{
                List<Customer> list = (List<Customer>)new XmlSerializer(typeof(List<Customer>)).Deserialize(reader);
                list.Sort((a, b) => a.Name.CompareTo(b.Name));
			    CachedCustomerList = list;
                return list;
			}
        }

        static List<Product> GetProductList()
        {
            string dataFilePath = Path.Combine(AppPath, Path.Combine("Xml", "Products.xml"));

			var loadedData = XDocument.Load(dataFilePath);
	        using (var reader = loadedData.Root.CreateReader())
			{
				return (List<Product>)new XmlSerializer(typeof(List<Product>)).Deserialize(reader);
			}
        }

        static List<Order> GetOrderList()
        {
            string dataFilePath = Path.Combine(AppPath, Path.Combine("Xml", "Orders.xml"));

			var loadedData = XDocument.Load(dataFilePath);
	        using (var reader = loadedData.Root.CreateReader())
			{
				return (List<Order>)new XmlSerializer(typeof(List<Order>)).Deserialize(reader);
			}
        }

        public static event EventHandler CustomersChanged;

        static void SaveCustomers(ObservableCollection<Customer> customers)
        {
            CachedCustomerList = customers;
            if (CustomersChanged != null)
                CustomersChanged(null, EventArgs.Empty);

#warning Hacked out!
            string dataFilePath = Path.Combine(AppPath, Path.Combine("Xml", "Customers.xml"));
            using (StreamWriter writer = new StreamWriter(dataFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Customer>));
                serializer.Serialize(writer, customers); 
            }
        }

        static void SaveContacts(string customer, ObservableCollection<Contact> contacts)
        {
            Customer instance = GetCustomer(customer);
            instance.Contacts = contacts;
            UpdateCustomer(instance);
        }

        static void SaveOrders(List<Order> orders)
        {
            string dataFilePath = Path.Combine(AppPath, Path.Combine("Xml", "Orders.xml"));

			using (StreamWriter writer = new StreamWriter(dataFilePath))
            {
                var serializer = new XmlSerializer(typeof(List<Order>));
                serializer.Serialize(writer, orders);
            }
        }
    }
     */
}

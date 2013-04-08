using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.ResourceLoader;
using CustomerManagement.AutoViews.Core.Interfaces.Models;

namespace CustomerManagement.AutoViews.Core.Models
{
    public class SimpleDataStore 
        : IDataStore
          
    {
        public SimpleDataStore()
        {
            Load();
        }

        public void UpdateCustomer(Customer customer)
        {
            var toUpdate = _customers.First(c => c.ID == customer.ID);
            toUpdate.CloneFrom(customer);
            Save();
        }

        public void DeleteCustomer(string customerId)
        {
            _customers.Remove(_customers.First(c => c.ID == customerId));
            Save();
        }

        public void CreateCustomer(Customer customer)
        {
            _customers.Add(customer);
            Save();
        }

        public Customer GetCustomer(string customerId)
        {
            return _customers.First(c => c.ID == customerId);
        }

        private const string StoreFileName = "customers.xml";
        private const string ResourceFileName = "Xml/Customers.xml";

        private void Load()
        {
            var fileService = Mvx.Resolve<IMvxFileStore>();
            if (!fileService.TryReadBinaryFile(StoreFileName, LoadFrom))
            {
                var resourceLoader = Mvx.Resolve<IMvxResourceLoader>();
                resourceLoader.GetResourceStream(ResourceFileName, (inputStream) => LoadFrom(inputStream));
            }
        }

        private bool LoadFrom(Stream inputStream)
        {
            try
            {
                var loadedData = XDocument.Load(inputStream);
                if (loadedData.Root == null)
                    return false;

                using (var reader = loadedData.Root.CreateReader())
                {
                    var list = (List<Customer>)new XmlSerializer(typeof(List<Customer>)).Deserialize(reader);
                    _customers = new SimpleObservableCollection<Customer>(list);
                    return true;
                }
            }
            catch (Exception exception)
            {
                CustomerManagementTrace.Trace("Problem loading customer list {0}", exception.ToLongString());
                return false;
            }
        }

        private void Save()
        {
            var fileService = Mvx.Resolve<IMvxFileStore>();
            fileService.WriteFile(StoreFileName, (stream) =>
                                                     {
                                                         var serializer = new XmlSerializer(typeof(List<Customer>));
                                                         serializer.Serialize(stream, _customers.ToList());
                                                     });
        }

        private IObservableCollection<Customer> _customers; 
        public IObservableCollection<Customer> Customers
        {
            get { return _customers; }
        }
    }
}
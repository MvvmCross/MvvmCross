using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.PhoneCall;
using Cirrious.MvvmCross.Plugins.WebBrowser;
using Cirrious.MvvmCross.ViewModels;
using CrossUI.Core.Descriptions;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core.ViewModels
{
    public class DetailsCustomerViewModel 
        : BaseViewModel        
    {
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            private set { _customer = value; RaisePropertyChanged("Customer"); }
        }

        public void Init(string customerId)
        {
            Customer = DataStore.GetCustomer(customerId);
        }

        public ICommand EditCommand
        {
            get
            {
                return new MvxCommand(DoEdit);
            }
        }

        public void DoEdit()
        {
            ShowViewModel<EditCustomerViewModel>(new Dictionary<string, string>()
                                                       {
                                                           {"customerId", Customer.ID}
                                                       });
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new MvxCommand(DoDelete);
            }
        }

        public void DoDelete()
        {
            try
            {
                DeleteCustomer();
                RequestClose();
            }
            catch (Exception exception)
            {
#warning TODO - how to send error messages?
            }
        }

        public ICommand ShowWebsiteCommand
        {
            get
            {
                return new MvxCommand(() =>
                                               {
                                                   Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader.Instance.EnsureLoaded();
                                                   Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(Customer.Website);
                                               });
            }
        }

        public ICommand CallCustomerCommand
        {
            get
            {
                return new MvxCommand(() =>
                                               {
                                                   Cirrious.MvvmCross.Plugins.PhoneCall.PluginLoader.Instance.EnsureLoaded();
                                                   Mvx.Resolve<IMvxPhoneCallTask>().MakePhoneCall(Customer.Name, Customer.PrimaryPhone);
                                               });
            }
        }

        public ICommand ShowOnMapCommand
        {
            get
            {
                return new MvxCommand(() =>
                                               {
                                                   Cirrious.MvvmCross.Plugins.WebBrowser.PluginLoader.Instance.EnsureLoaded();
                                                   string googleAddress = string.Format("{0} {1}\n{2}, {3}  {4}",
                                                                                        Customer.PrimaryAddress.
                                                                                            Street1,
                                                                                        Customer.PrimaryAddress.
                                                                                            Street2,
                                                                                        Customer.PrimaryAddress.
                                                                                            City,
                                                                                        Customer.PrimaryAddress.
                                                                                            State,
                                                                                        Customer.PrimaryAddress.
                                                                                            Zip);
                                                   googleAddress = Uri.EscapeUriString(googleAddress);

                                                   string url = string.Format("http://maps.google.com/maps?q={0}",
                                                                              googleAddress);
                                                   Mvx.Resolve<IMvxWebBrowserTask>().ShowWebPage(url);
                                               });
            }
        }

#warning Broken Code - also should probably use a service to do the save, not the static
        private void DeleteCustomer()
        {
            DataStore.DeleteCustomer(Customer.ID);
        }
    }
}

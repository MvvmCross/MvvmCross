using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Services;
using Cirrious.MvvmCross.ShortNames;

namespace CustomerManagement.ViewModels
{
    public class DetailsCustomerViewModel 
        : BaseCustomerViewModel
        , IMvxServiceConsumer<IMvxWebBrowserTask>
        , IMvxServiceConsumer<IMvxPhoneCallTask>
    {
        public IMvxCommand EditCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
                                                  RequestNavigate<EditCustomerViewModel>(new StringDict()
                                                                                         {
                                                                                            { "customerId", Customer.ID }
                                                                                         });
                                               });
            }
        }

        public IMvxCommand DeleteCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                {
                    try
                    {
                        DeleteCustomer();
                        RequestNavigateBack();
                    }
                    catch (Exception exception)
                    {
#warning TODO - how to send error messages?
                    }
                });
            }
        }

        public IMvxCommand ShowWebsiteCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
                                                   this.GetService<IMvxWebBrowserTask>().ShowWebPage(Customer.Website);
                                               });
            }
        }

        public IMvxCommand CallCustomerCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
                                                   this.GetService<IMvxPhoneCallTask>().MakePhoneCall(Customer.Name, Customer.PrimaryPhone);
                                               });
            }
        }

        public IMvxCommand ShowOnMapCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
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
                                                   this.GetService<IMvxWebBrowserTask>().ShowWebPage(url);
                                               });
            }
        }

    }
}

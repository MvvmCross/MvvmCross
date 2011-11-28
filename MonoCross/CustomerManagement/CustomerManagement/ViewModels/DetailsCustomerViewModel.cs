using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.ShortNames;

namespace CustomerManagement.ViewModels
{
    public class DetailsCustomerViewModel : BaseCustomerViewModel
    {
        public IMvxCommand EditCommand
        {
            get
            {
#warning TODO - need to do EDIT better
                return new MvxRelayCommand(() =>
                                               {
                                                  RequestNavigate<EditCustomerViewModel>("Edit",
                                                                                         new StringDict()
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
#warning TODO - need to change DELETE to work, then do back!
#warning TODO - need to change DELETE to work, then do back!
#warning TODO - need to change DELETE to work, then do back!
#warning TODO - need to change DELETE to work, then do back!
#warning TODO - need to change DELETE to work, then do back!
                    RequestNavigateBack();
                });
            }
        }

        public IMvxCommand ShowWebsiteCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
#warning Windows Phone specific code here - need services!
                                                   var webBrowserTask = new Microsoft.Phone.Tasks.WebBrowserTask();
                                                   webBrowserTask.Uri = new Uri(Customer.Website);
                                                   webBrowserTask.Show();                                                   
                                               });
            }
        }

        public IMvxCommand CallCustomerCommand
        {
            get
            {
#warning Windows Phone specific code here - need services!
                return new MvxRelayCommand(() =>
                                               {
                                                   var pct = new Microsoft.Phone.Tasks.PhoneCallTask();
                                                   pct.DisplayName = Customer.Name;
                                                   pct.PhoneNumber = Customer.PrimaryPhone;
                                                   pct.Show();
                                               });
            }
        }

        public IMvxCommand ShowOnMapCommand
        {
            get
            {
#warning Windows Phone specific code here - need services!
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

                                                   var webBrowserTask = new Microsoft.Phone.Tasks.WebBrowserTask();
                                                   webBrowserTask.Uri = new Uri(url);
                                                   webBrowserTask.Show();
                                               });
            }
        }

    }
}

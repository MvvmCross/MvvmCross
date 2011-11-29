using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace CustomerManagement.ViewModels
{
    public class NewCustomerViewModel : BaseCustomerViewModel
    {
        public IMvxCommand SaveCommand
        {
            get
            {
                return new MvxRelayCommand(() =>
                                               {
                                                   try
                                                   {
                                                       AddNewCustomer();
                                                       RequestNavigateBack();
                                                   }
                                                   catch (Exception exception)
                                                   {
#warning TODO - how to send error messages?
                                                   }
                                               });
            }
        }
    }
}

using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace CustomerManagement.Core.ViewModels
{
    public class NewCustomerViewModel
        : BaseEditCustomerViewModel
    {
        public NewCustomerViewModel()
            : base(null)
        {            
        }

        public override IMvxCommand SaveCommand
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

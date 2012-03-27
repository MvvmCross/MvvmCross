using System;
using Cirrious.MvvmCross.Commands;
using Cirrious.MvvmCross.Interfaces.Commands;

namespace CustomerManagement.Core.ViewModels
{
    public class EditCustomerViewModel : BaseEditCustomerViewModel
    {
        public EditCustomerViewModel(string customerId)
            : base(customerId)
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
                                                       UpdateCustomer();
                                                       RequestClose(this);
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

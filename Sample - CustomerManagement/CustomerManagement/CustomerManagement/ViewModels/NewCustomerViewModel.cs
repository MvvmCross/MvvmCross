using System;
using System.Windows.Input;
using Cirrious.MvvmCross.Commands;


namespace CustomerManagement.Core.ViewModels
{
    public class NewCustomerViewModel
        : BaseEditCustomerViewModel
    {
        public NewCustomerViewModel()
            : base(null)
        {            
        }

        public override void DoSave()
        {
            try
            {
                AddNewCustomer();
                RequestClose(this);
            }
            catch (Exception exception)
            {
#warning TODO - how to send error messages?
            }
        }
    }
}

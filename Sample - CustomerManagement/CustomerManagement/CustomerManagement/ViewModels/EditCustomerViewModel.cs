using System;
using System.Windows.Input;

namespace CustomerManagement.Core.ViewModels
{
    public class EditCustomerViewModel : BaseEditCustomerViewModel
    {
        public override void DoSave()
        {
            try
            {
                UpdateCustomer();
                RequestClose();
            }
            catch (Exception exception)
            {
#warning TODO - how to send error messages?
            }
        }
    }
}

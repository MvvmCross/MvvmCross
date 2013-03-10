using System;

namespace CustomerManagement.AutoViews.Core.ViewModels
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

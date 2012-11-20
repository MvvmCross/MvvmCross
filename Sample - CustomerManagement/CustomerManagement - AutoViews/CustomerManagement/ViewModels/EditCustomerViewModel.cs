using System;

namespace CustomerManagement.AutoViews.Core.ViewModels
{
    public class EditCustomerViewModel : BaseEditCustomerViewModel
    {
        public EditCustomerViewModel(string customerId)
            : base(customerId)
        {            
        }

        public override void DoSave()
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
        }
    }
}

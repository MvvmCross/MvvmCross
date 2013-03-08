﻿using System;

namespace CustomerManagement.AutoViews.Core.ViewModels
{
    public class NewCustomerViewModel
        : BaseEditCustomerViewModel
    {
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

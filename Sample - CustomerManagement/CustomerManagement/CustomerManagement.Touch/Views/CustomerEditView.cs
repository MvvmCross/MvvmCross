using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using CustomerManagement;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Touch
{
    public class CustomerEditView: BaseCustomerEditView<EditCustomerViewModel>
	{
        public CustomerEditView(MvxShowViewModelRequest request)
            : base(request)
	    {	        
	    }
	}
}


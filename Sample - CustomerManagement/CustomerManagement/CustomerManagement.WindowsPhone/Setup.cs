using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using CustomerManagement.Core.ViewModels;
using Microsoft.Phone.Controls;

namespace CustomerManagement.WindowsPhone
{
    public class Setup 
        : MvxBaseWindowsPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame)
            : base(rootFrame)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app = new Core.App();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>()
                       {
                            { typeof(CustomerListViewModel), typeof(CustomerListView)},
                            { typeof(DetailsCustomerViewModel), typeof(CustomerView)},
                            { typeof(EditCustomerViewModel), typeof(CustomerEditView)},
                            { typeof(NewCustomerViewModel), typeof(CustomerNewView)},
                       };
        }
    }
}

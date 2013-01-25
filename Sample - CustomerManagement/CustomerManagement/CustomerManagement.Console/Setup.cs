using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Console.Views;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Console
{
    public class Setup
        : MvxBaseConsoleSetup
    {
        #region Overrides of MvxBaseSetup

        protected override MvxApplication CreateApp()
        {
            var app = new CustomerManagement.Core.App();
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

        #endregion
    }
}
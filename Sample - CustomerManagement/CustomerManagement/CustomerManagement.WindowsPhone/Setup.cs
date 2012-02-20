using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Conventions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Cirrious.MvvmCross.WindowsPhone.Platform;
using Cirrious.MvvmCross.WindowsPhone.Views;
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

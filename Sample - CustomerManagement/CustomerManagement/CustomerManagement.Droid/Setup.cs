using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Core;
using CustomerManagement.Core.ViewModels;
using CustomerManagement.Droid.Views;

namespace CustomerManagement.Droid
{
    public class Setup 
        : MvxBaseAndroidSetup
          , IMvxServiceProducer<IMvxStartNavigation>
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        #region Overrides of MvxBaseSetup

        protected override MvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }

        protected override IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return new Dictionary<Type, Type>()
                       {
                           { typeof(CustomerListViewModel), typeof(CustomerListView)},
                           { typeof(DetailsCustomerViewModel), typeof(DetailsCustomerView)},
                           { typeof(EditCustomerViewModel), typeof(CustomerEditView)},
                           { typeof(NewCustomerViewModel), typeof(CustomerNewView)},
                       };
        }

        protected override void InitializeLastChance()
        {
            var bindingSetup = new CustomerManagementBindingSetup();
            bindingSetup.DoRegistration();

            base.InitializeLastChance();
        }

        #endregion

        public override string ExecutableNamespace
        {
            get { return "CustomerManagement.Droid"; }
        }

        public override Assembly ExecutableAssembly
        {
            get { return this.GetType().Assembly; }
        }
    }
}
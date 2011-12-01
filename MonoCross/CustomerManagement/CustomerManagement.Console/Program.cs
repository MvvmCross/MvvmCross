using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Console.Interfaces;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using CustomerManagement;
using CustomerManagement.Console.Views;
using CustomerManagement.Shared;
using CustomerManagement.Shared.Model;
using CustomerManagement.ViewModels;

namespace CustomerManagement.Console
{
    public class Setup
        : MvxBaseConsoleSetup
        , IMvxServiceProducer<IMvxStartNavigation>
    {
        #region Overrides of MvxBaseSetup

        protected override MvxApplication CreateApp()
        {
            var app = new CustomerManagement.App();
            this.RegisterServiceInstance<IMvxStartNavigation>(app);
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

    class Program
        : IMvxServiceConsumer<IMvxStartNavigation>
        , IMvxServiceConsumer<IMvxMessagePump>
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }

        void Run()
        {
            // initialize app
            var setup = new Setup();
            setup.Initialize();

            // trigger the first navigate...
            var starter = this.GetService<IMvxStartNavigation>();
            starter.Start();

            // enter the run loop
            var pump = this.GetService<IMvxMessagePump>();
            pump.Run();
        }
    }
}

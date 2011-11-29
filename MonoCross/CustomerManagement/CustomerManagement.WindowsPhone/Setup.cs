using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cirrious.MvvmCross.Conventions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces;
using Cirrious.MvvmCross.Interfaces.Application;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.IoC;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.WindowsPhone.Interfaces;
using Cirrious.MvvmCross.WindowsPhone.Views;
using CustomerManagement.ViewModels;
using Microsoft.Phone.Controls;

namespace CustomerManagement.WindowsPhone
{
    public class Setup 
        : IMvxServiceProducer<IMvxApplicationTitle>
        , IMvxServiceProducer<IMvxStartNavigation>
        , IMvxServiceProducer<IMvxViewModelLocatorFinder>
        , IMvxServiceProducer<IMvxViewModelLocatorStore>
        , IMvxServiceProducer<IMvxViewsContainer>
        , IMvxServiceProducer<IMvxViewDispatcherProvider>
        , IMvxServiceProducer<IMvxWindowsPhoneViewModelRequestTranslator>
        , IMvxServiceConsumer<IMvxViewsContainer>
    {
        public void Build(PhoneApplicationFrame rootFrame)
        {
            // initialise the IoC service registry
            // this also pulls in all platform services too
            MvxOpenNetCfServiceProviderSetup.Initialize();

            // initialize conventions
            MvxDefaultConventionSetup.Initialize();

            // initialize app
            InitaliseApp();

            // initialize container
            InitializeContainer(rootFrame);

            // initialize views
            InitializeViews();
        }

        private void InitializeViews()
        {
            var container = this.GetService<IMvxViewsContainer>();

            container.Add<CustomerListViewModel>(typeof(CustomerListView));
            container.Add<DetailsCustomerViewModel>(typeof(CustomerView));
            container.Add<EditCustomerViewModel>(typeof(CustomerEditView));
        }

        private MvxPhoneViewsContainer InitializeContainer(PhoneApplicationFrame rootFrame)
        {
            var container = new MvxPhoneViewsContainer(rootFrame);

            this.RegisterServiceInstance<IMvxViewsContainer>(container);
            this.RegisterServiceInstance<IMvxViewDispatcherProvider>(container);
            this.RegisterServiceInstance<IMvxWindowsPhoneViewModelRequestTranslator>(container);
            return container;
        }

        private void InitaliseApp()
        {
            var app = new CustomerManagement.App();
            this.RegisterServiceInstance<IMvxApplicationTitle>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorFinder>(app);
            this.RegisterServiceInstance<IMvxViewModelLocatorStore>(app);
            this.RegisterServiceInstance<IMvxStartNavigation>(app);
        }
    }
}

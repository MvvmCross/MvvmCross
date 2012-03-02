using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core
{
    public class App 
        : MvxApplication
        , IMvxServiceProducer<IMvxStartNavigation>
        , IMvxServiceProducer<IDataStore>
    {
        public App()
        {
            // set up the model
            var dataStore = new SimpleDataStore();
            this.RegisterServiceInstance<IDataStore>(dataStore);

            // set the start object
            var startApplicationObject = new StartApplicationObject();
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
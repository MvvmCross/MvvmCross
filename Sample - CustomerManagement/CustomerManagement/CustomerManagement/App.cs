using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Core.Models;

namespace CustomerManagement.Core
{
    public class App 
        : MvxApplication
        , IMvxServiceProducer
    {
        public App()
        {
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();

            // set up the model
            var dataStore = new SimpleDataStore();
            this.RegisterServiceInstance<IDataStore>(dataStore);

            // set the start object
            var startApplicationObject = new StartApplicationObject();
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
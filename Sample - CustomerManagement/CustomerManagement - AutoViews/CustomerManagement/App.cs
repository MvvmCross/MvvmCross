using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces.Models;
using CustomerManagement.AutoViews.Core.Models;

namespace CustomerManagement.AutoViews.Core
{
    public class App 
        : MvxApplication
        , IMvxProducer
    {
        public App()
        {
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();

            // set up the model
            var dataStore = new SimpleDataStore();
            this.RegisterSingleton<IDataStore>(dataStore);

            // set the start object
            var startApplicationObject = new StartApplicationObject();
            this.RegisterSingleton<IMvxStartNavigation>(startApplicationObject);
        }
    }
}
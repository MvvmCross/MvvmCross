using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using CustomerManagement.AutoViews.Core.Interfaces.Models;
using CustomerManagement.AutoViews.Core.Models;
using CustomerManagement.AutoViews.Core.ViewModels;

namespace CustomerManagement.AutoViews.Core
{
    public class App 
        : MvxApplication
        
    {
        public App()
        {
            Cirrious.MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            Cirrious.MvvmCross.Plugins.ResourceLoader.PluginLoader.Instance.EnsureLoaded();

            // set up the model
            var dataStore = new SimpleDataStore();
            Mvx.RegisterSingleton<IDataStore>(dataStore);

            // set the start object
            RegisterAppStart<CustomerListViewModel>();
        }
    }
}
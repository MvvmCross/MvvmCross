using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core
{
    public class TwitterSearchApp
        : MvxApplication
        , IMvxServiceProducer
    {
        public TwitterSearchApp()
        {
            InitaliseServices();
            InitialiseStartNavigation();
            InitialisePlugIns();
        }

        private void InitaliseServices()
        {
            this.RegisterServiceInstance<ITwitterSearchProvider>(new TwitterSearchProvider());
        }

        private void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartNavigation();
            this.RegisterServiceInstance<IMvxStartNavigation>(startApplicationObject);
        }

        private void InitialisePlugIns()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}

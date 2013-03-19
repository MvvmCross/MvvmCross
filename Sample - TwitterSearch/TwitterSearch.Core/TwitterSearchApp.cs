using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core
{
    public class TwitterSearchApp
        : MvxApplication
        
    {
        public TwitterSearchApp()
        {
            InitaliseServices();
            InitialiseStartNavigation();
            InitialisePlugIns();
        }

        private void InitaliseServices()
        {
            Mvx.RegisterSingleton<ITwitterSearchProvider>(new TwitterSearchProvider());
        }

        private void InitialiseStartNavigation()
        {
            var startApplicationObject = new StartNavigation();
            Mvx.RegisterSingleton<IMvxStartNavigation>(startApplicationObject);
        }

        private void InitialisePlugIns()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}

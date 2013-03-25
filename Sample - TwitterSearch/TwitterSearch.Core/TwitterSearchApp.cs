using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;
using TwitterSearch.Core.ViewModels;

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
            RegisterAppStart<HomeViewModel>();
        }

        private void InitialisePlugIns()
        {
            Cirrious.MvvmCross.Plugins.Visibility.PluginLoader.Instance.EnsureLoaded();
        }
    }
}

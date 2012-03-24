using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using TwitterSearch.Core.Interfaces;
using TwitterSearch.Core.Models;

namespace TwitterSearch.Core
{
    public class TwitterSearchApp
        : MvxApplication
        , IMvxServiceProducer<IMvxStartNavigation>
        , IMvxServiceProducer<ITwitterSearchProvider>
    {
        public TwitterSearchApp()
        {
            InitaliseServices();
            InitialiseStartNavigation();
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
    }
}

using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using TwitterSearch.Core.Interfaces;

namespace TwitterSearch.Test.Mocks
{
    public class MockApp
        : MvxApplication
        , IMvxServiceProducer<ITwitterSearchProvider>
    {
        public MockApp()
        {
            // nothing to do        
        }

        public MockApp(ITwitterSearchProvider provider)
        {
            // nothing to do        
            this.RegisterServiceInstance(provider);
        }
    }
}

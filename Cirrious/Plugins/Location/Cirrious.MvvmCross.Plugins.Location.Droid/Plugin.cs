using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Location.Droid
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxGeoLocationWatcher>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxGeoLocationWatcher>(new MvxAndroidGeoLocationWatcher());
        }

        #endregion
    }
}
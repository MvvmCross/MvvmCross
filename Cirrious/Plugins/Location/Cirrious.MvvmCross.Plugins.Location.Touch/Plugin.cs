using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Location.Touch
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxGeoLocationWatcher>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxGeoLocationWatcher>(new MvxTouchGeoLocationWatcher());
        }

        #endregion
    }
}
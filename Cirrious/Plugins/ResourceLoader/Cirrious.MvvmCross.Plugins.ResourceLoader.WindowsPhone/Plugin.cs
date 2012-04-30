using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxResourceLoader>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxResourceLoader, MvxWindowsPhoneResourceLoader>();
        }

        #endregion
    }
}
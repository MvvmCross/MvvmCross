using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class PluginLoader
        : IMvxPluginLoader
        , IMvxServiceProducer
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        private bool _loaded;

        #region Implementation of IMvxPluginLoader

        public void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            this.RegisterServiceType<IMessenger, MessengerHub>();
            _loaded = true;
        }

        #endregion
    }
}

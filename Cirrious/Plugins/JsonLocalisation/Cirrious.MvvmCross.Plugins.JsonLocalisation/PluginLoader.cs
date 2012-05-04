using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public class PluginLoader
        : IMvxPluginLoader
        , IMvxServiceProducer<IMvxTextProviderBuilder>
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

            this.RegisterServiceType<IMvxTextProviderBuilder, MvxTextProviderBuilder>();
            _loaded = true;
        }

        #endregion
    }
}

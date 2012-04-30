using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.ResourceLoader.Touch
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxResourceLoader>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            Plugins.File.PluginLoader.Instance.EnsureLoaded();

            this.RegisterServiceType<IMvxResourceLoader, MvxTouchResourceLoader>();
        }

        #endregion
    }
}
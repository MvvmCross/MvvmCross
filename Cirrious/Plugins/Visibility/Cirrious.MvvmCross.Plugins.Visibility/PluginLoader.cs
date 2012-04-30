using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Visibility
{
    public class PluginLoader
        : IMvxPluginLoader
        , IMvxServiceConsumer<IMvxPluginManager>
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        #region Implementation of IMvxPluginLoader

        public void EnsureLoaded()
        {
            var manager = this.GetService<IMvxPluginManager>();
            manager.EnsureLoaded<PluginLoader>();
        }

        #endregion
    }
}

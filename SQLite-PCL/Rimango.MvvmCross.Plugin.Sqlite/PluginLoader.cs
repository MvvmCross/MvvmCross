using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Rimango.MvvmCross.Plugin.Sqlite
{
    public class PluginLoader : IMvxPluginLoader 
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        public void EnsureLoaded()
        {
            var manager = Mvx.Resolve<IMvxPluginManager>();
            manager.EnsurePlatformAdaptionLoaded<PluginLoader>();
        }
    }
}

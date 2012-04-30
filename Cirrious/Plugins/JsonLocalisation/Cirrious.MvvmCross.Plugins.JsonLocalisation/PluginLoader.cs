using Cirrious.MvvmCross.Interfaces.Plugins;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public class PluginLoader
        : IMvxPluginLoader
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

            Instance.EnsureLoaded();
            _loaded = true;
        }

        #endregion
    }
}

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.SoundEffects.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            ResourceLoader.PluginLoader.Instance.EnsureLoaded();

            this.RegisterServiceType<IMvxSoundEffectLoader, MvxSoundEffectObjectLoader>();
        }

        #endregion
    }
}
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Share.Touch
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxShareTask>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxShareTask, MvxShareTask>();
        }

        #endregion
    }
}
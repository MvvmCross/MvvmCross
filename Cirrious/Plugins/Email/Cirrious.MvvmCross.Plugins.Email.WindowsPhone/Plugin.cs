using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Email.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxComposeEmailTask>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxComposeEmailTask, MvxComposeEmailTask>();
        }

        #endregion
    }
}
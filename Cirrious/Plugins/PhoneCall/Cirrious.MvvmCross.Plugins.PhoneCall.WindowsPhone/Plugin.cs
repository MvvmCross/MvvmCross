using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.PhoneCall.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxPhoneCallTask>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxPhoneCallTask, MvxPhoneCallTask>();
        }

        #endregion
    }
}
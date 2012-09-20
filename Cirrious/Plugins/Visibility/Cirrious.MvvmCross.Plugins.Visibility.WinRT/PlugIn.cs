using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Visibility.WinRT
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxNativeVisibility>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxNativeVisibility>(new MvxWinRTVisibility());
        }

        #endregion
    }
}
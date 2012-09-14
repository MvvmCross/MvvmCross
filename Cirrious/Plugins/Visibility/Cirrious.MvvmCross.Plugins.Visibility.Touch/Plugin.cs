using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Visibility;

namespace Cirrious.MvvmCross.Plugins.Visibility.Touch
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxNativeVisibility>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<IMvxNativeVisibility>(new MvxTouchVisibility());
        }

        #endregion
    }
}
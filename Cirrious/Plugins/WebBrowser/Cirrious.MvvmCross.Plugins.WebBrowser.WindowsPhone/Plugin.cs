using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.WebBrowser.WindowsPhone
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxWebBrowserTask>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxWebBrowserTask, MvxWebBrowserTask>();
        }

        #endregion
    }
}
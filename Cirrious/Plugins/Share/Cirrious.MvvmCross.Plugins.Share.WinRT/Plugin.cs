using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Share.WinRT
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            // nothing to do - WinRT does not currently do share this way...
        }

        #endregion
    }
}
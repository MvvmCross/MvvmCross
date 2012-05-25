#warning Copyright needed

using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.File.WinRT
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<IMvxSimpleFileStoreService>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceType<IMvxSimpleFileStoreService, MvxBlockingWinRTFileStoreService>();
        }

        #endregion
    }
}

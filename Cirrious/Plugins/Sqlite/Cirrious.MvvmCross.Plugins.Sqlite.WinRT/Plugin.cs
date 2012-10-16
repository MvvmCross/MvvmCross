using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Plugins.Sqlite.WinRT
{
    public class Plugin
        : IMvxPlugin
        , IMvxServiceProducer<ISQLiteConnectionFactory>
    {
        #region Implementation of IMvxPlugin

        public void Load()
        {
            this.RegisterServiceInstance<ISQLiteConnectionFactory>(new MvxWinRTSQLiteConnectionFactory());
        }

        #endregion
    }
}
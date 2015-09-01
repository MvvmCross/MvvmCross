using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Rimango.MvvmCross.Plugin.Sqlite.WindowsCommon
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
        }
    }
}
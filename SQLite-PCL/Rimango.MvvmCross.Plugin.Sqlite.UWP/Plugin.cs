using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Rimango.MvvmCross.Plugin.Sqlite.UWP
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
        }
    }
}
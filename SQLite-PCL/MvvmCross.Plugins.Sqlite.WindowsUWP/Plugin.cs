using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.Sqlite.WindowsUWP
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
        }
    }
}
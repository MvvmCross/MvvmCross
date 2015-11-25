using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.Sqlite.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSqliteConnectionFactory, DroidSqliteConnectionFactory>();
        }
    }
}
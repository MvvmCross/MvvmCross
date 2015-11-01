using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugins.Sqlite.Wpf
{
	public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			Mvx.RegisterType<IMvxSqliteConnectionFactory, WindowsSqliteConnectionFactory>();
		}
	}
}
using System;
using System.IO;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;

namespace MvvmCross.Plugins.Sqlite.Wpf
{
	public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
	{
		public override ISQLitePlatform CurrentPlattform
		{
			get
			{
				return new SQLitePlatformWin32();
			}
		}

		public override string GetPlattformDatabasePath(string databaseName)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), databaseName);
		}
	}
}

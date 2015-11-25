using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System;
using System.IO;

namespace MvvmCross.Plugins.Sqlite.Wpf
{
    public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override ISQLitePlatform CurrentPlattform => new SQLitePlatformWin32();

        public override string GetPlattformDatabasePath(string databaseName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), databaseName);
        }
    }
}
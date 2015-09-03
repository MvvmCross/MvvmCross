using System.IO;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

namespace MvvmCross.Plugin.Sqlite.UWP
{
    public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override ISQLitePlatform CurrentPlattform => new SQLitePlatformWinRT();
        public override string GetPlattformDatabasePath(string databaseName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}
using System.IO;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WindowsPhone8;

namespace MvvmCross.Plugins.Sqlite.WindowsPhone
{
    public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override ISQLitePlatform CurrentPlattform => new SQLitePlatformWP8();

        public override string GetPlattformDatabasePath(string databaseName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}
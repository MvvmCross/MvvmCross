using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using System;
using System.IO;

namespace MvvmCross.Plugins.Sqlite.Touch
{
    public class TouchSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override ISQLitePlatform CurrentPlattform => new SQLitePlatformIOS();

        public override string GetPlattformDatabasePath(string databaseName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, databaseName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;

namespace Rimango.MvvmCross.Plugin.Sqlite.Touch
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

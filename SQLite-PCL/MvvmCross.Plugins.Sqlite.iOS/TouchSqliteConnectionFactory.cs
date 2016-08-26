using System;
using System.IO;

namespace MvvmCross.Plugins.Sqlite.iOS
{
    public class TouchSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override string GetPlattformDatabasePath(string databaseName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, databaseName);
        }
    }
}
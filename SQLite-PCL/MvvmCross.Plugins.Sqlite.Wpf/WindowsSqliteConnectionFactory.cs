using System;
using System.IO;

namespace MvvmCross.Plugins.Sqlite.Wpf
{
    public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    { 
        public override string GetPlattformDatabasePath(string databaseName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), databaseName);
        }
    }
}
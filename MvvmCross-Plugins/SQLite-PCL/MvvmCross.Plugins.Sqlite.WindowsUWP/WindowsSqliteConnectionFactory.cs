using System.IO;

namespace MvvmCross.Plugins.Sqlite.WindowsUWP
{
    public class WindowsSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {

        public override string GetPlattformDatabasePath(string databaseName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}
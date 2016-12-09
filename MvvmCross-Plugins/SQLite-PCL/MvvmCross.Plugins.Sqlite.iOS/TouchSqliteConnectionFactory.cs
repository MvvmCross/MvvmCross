using System;
using System.IO;

namespace MvvmCross.Plugins.Sqlite.iOS
{
    [Preserve(AllMembers = true)]
	public class TouchSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override string GetPlattformDatabasePath(string databaseName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, databaseName);
        }
    }
}
using System.IO;
using Environment = System.Environment;

namespace MvvmCross.Plugins.Sqlite.Droid
{
    [Preserve(AllMembers = true)]
	public class DroidSqliteConnectionFactory : MvxSqliteConnectionFactoryBase
    {
        public override string GetPlattformDatabasePath(string databaseName)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, databaseName);
        }
    }
}
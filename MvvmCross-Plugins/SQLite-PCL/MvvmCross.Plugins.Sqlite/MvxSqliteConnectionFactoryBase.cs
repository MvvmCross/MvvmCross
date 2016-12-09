using SQLite;

namespace MvvmCross.Plugins.Sqlite
{
    public abstract class MvxSqliteConnectionFactoryBase : IMvxSqliteConnectionFactory
    {
        public abstract string GetPlattformDatabasePath(string databaseName);

        protected string GetConnectionString(SqLiteConfig config, bool prefixPlatformPath = true)
        {
            var path = prefixPlatformPath ? GetPlattformDatabasePath(config.DatabaseName) : config.DatabaseName;
            return path;
        }

        public SQLiteConnection GetConnection(string databaseName, bool prefixPlatformPath = true)
        {
            var config = new SqLiteConfig(databaseName);
            var databasePath = GetConnectionString(config, prefixPlatformPath);

            return new SQLiteConnection(databasePath, config.OpenFlags, config.StoreDateTimeAsTicks);
        }

        public SQLiteConnection GetConnection(SqLiteConfig config, bool prefixPlatformPath = true)
        {
            var databasePath = GetConnectionString(config, prefixPlatformPath);

            return new SQLiteConnection(databasePath, config.OpenFlags, config.StoreDateTimeAsTicks);
        }

        public SQLiteAsyncConnection GetAsyncConnection(string databaseName, bool prefixPlatformPath = true)
        {
            var config = new SqLiteConfig(databaseName);
            var databasePath = GetConnectionString(config, prefixPlatformPath);

            return new SQLiteAsyncConnection(databasePath, config.OpenFlags, config.StoreDateTimeAsTicks);
        }

        public SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config, bool prefixPlatformPath = true)
        {
            var databasePath = GetConnectionString(config, prefixPlatformPath);

            return new SQLiteAsyncConnection(databasePath, config.OpenFlags, config.StoreDateTimeAsTicks);
        }
    }
}
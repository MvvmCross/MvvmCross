using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace MvvmCross.Plugins.Sqlite
{
    public abstract class MvxSqliteConnectionFactoryBase : IMvxSqliteConnectionFactory
    {
        public abstract ISQLitePlatform CurrentPlattform { get; }

        public abstract string GetPlattformDatabasePath(string databaseName);

        protected SQLiteConnectionString GetConnectionString(SqLiteConfig config, bool appendPlatformPath)
        {
            var path = appendPlatformPath ? GetPlattformDatabasePath(config.DatabaseName) : config.DatabaseName;
            return new SQLiteConnectionString(path, config.StoreDateTimeAsTicks, config.BlobSerializer, config.ContractResolver);
        }

        public SQLiteConnection GetConnection(string databaseName, bool appendPlatformPath = true)
        {
            return GetConnection(new SqLiteConfig(databaseName), appendPlatformPath);
        }

        public SQLiteConnection GetConnection(SqLiteConfig config, bool appendPlatformPath = true)
        {
            var connectionString = GetConnectionString(config, appendPlatformPath);
            return new SQLiteConnection(CurrentPlattform, connectionString.DatabasePath, connectionString.StoreDateTimeAsTicks, connectionString.Serializer);
        }

        public SQLiteConnectionWithLock GetConnectionWithLock(string databaseName, bool appendPlatformPath = true)
        {
            return GetConnectionWithLock(new SqLiteConfig(databaseName), appendPlatformPath);
        }

        public SQLiteConnectionWithLock GetConnectionWithLock(SqLiteConfig config, bool appendPlatformPath = true)
        {
            var connectionString = GetConnectionString(config, appendPlatformPath);
            return new SQLiteConnectionWithLock(CurrentPlattform, connectionString);
        }

        public SQLiteAsyncConnection GetAsyncConnection(string databaseName, bool appendPlatformPath = true)
        {
            return new SQLiteAsyncConnection(() => GetConnectionWithLock(databaseName, appendPlatformPath));
        }

        public SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config, bool appendPlatformPath = true)
        {
            return new SQLiteAsyncConnection(() => GetConnectionWithLock(config, appendPlatformPath));
        }
    }
}
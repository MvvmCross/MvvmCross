using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace Rimango.MvvmCross.Plugin.Sqlite
{
    public abstract class MvxSqliteConnectionFactoryBase : IMvxSqliteConnectionFactory
    {
        public abstract ISQLitePlatform CurrentPlattform { get; }
        public abstract string GetPlattformDatabasePath(string databaseName);
        public SQLiteConnectionWithLock GetConnectionWithLock(string databaseName)
        {
            return GetConnectionWithLock(new SqLiteConfig(databaseName));
        }

        public SQLiteAsyncConnection GetAsyncConnection(string databaseName)
        {
            return new SQLiteAsyncConnection(() => GetConnectionWithLock(databaseName));
        }

        public SQLiteConnection GetConnection(string databaseName)
        {
            return GetConnection(new SqLiteConfig(databaseName));
        }

        public SQLiteConnectionWithLock GetConnectionWithLock(SqLiteConfig config)
        {
            var connectionString = GetConnectionString(config);
            return new SQLiteConnectionWithLock(CurrentPlattform, connectionString);
        }

        public SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config)
        {
            return new SQLiteAsyncConnection(() => GetConnectionWithLock(config));
        }

        public SQLiteConnection GetConnection(SqLiteConfig config)
        {
            var connectionString = GetConnectionString(config);
            return new SQLiteConnection(CurrentPlattform, connectionString.DatabasePath, connectionString.StoreDateTimeAsTicks, connectionString.Serializer);
        }

        protected SQLiteConnectionString GetConnectionString(SqLiteConfig config)
        {
            var path = GetPlattformDatabasePath(config.DatabaseName);
            return new SQLiteConnectionString(path, config.StoreDateTimeAsTicks, config.BlobSerializer, config.ContractResolver);
        }
    }
}

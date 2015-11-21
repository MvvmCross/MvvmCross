using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace MvvmCross.Plugins.Sqlite
{
    public interface IMvxSqliteConnectionFactory
    {
        ISQLitePlatform CurrentPlattform { get; }

        SQLiteConnection GetConnection(string databaseName, bool appendPlatformPath = true);
        SQLiteConnection GetConnection(SqLiteConfig config, bool appendPlatformPath = true);

        SQLiteConnectionWithLock GetConnectionWithLock(string databaseName, bool appendPlatformPath = true);
        SQLiteConnectionWithLock GetConnectionWithLock(SqLiteConfig config, bool appendPlatformPath = true);

        SQLiteAsyncConnection GetAsyncConnection(string databaseName, bool appendPlatformPath = true);
        SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config, bool appendPlatformPath = true);
    }
}

using SQLite.Net;
using SQLite.Net.Async;

namespace MvvmCross.Plugins.Sqlite
{
    public interface IMvxSqliteConnectionFactory
    {
        SQLiteConnectionWithLock GetConnectionWithLock(string databaseName);
        SQLiteAsyncConnection GetAsyncConnection(string databaseName);
        SQLiteConnection GetConnection(string databaseName);
        SQLiteConnectionWithLock GetConnectionWithLock(SqLiteConfig config);
        SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config);
        SQLiteConnection GetConnection(SqLiteConfig config);
    }
}

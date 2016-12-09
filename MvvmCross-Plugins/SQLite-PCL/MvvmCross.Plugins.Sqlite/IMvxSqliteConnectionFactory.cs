using SQLite;

namespace MvvmCross.Plugins.Sqlite
{
    public interface IMvxSqliteConnectionFactory
    {
        SQLiteConnection GetConnection(string databaseName, bool prefixPlatformPath = true);

        SQLiteConnection GetConnection(SqLiteConfig config, bool prefixPlatformPath = true);

        SQLiteAsyncConnection GetAsyncConnection(string databaseName, bool prefixPlatformPath = true);

        SQLiteAsyncConnection GetAsyncConnection(SqLiteConfig config, bool prefixPlatformPath = true);
    }
}
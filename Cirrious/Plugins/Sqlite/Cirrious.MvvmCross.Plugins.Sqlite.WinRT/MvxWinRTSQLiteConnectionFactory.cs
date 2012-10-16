using System;
using System.IO;
using SQLite;

namespace Cirrious.MvvmCross.Plugins.Sqlite.WinRT
{
    public class MvxWinRTSQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public ISQLiteConnection Create(string address)
        {
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, address);
            return new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }
    }
}

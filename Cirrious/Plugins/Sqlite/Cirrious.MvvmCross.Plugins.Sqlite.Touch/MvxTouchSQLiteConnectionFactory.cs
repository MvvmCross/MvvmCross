using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Touch
{
    public class MvxTouchSQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public ISQLiteConnection Create(string address)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return new SQLiteConnection(Path.Combine(path, address));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Droid
{
    public class MvxDroidSQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public ISQLiteConnection Create(string address)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return new Connection(Path.Combine(path, address));
        }
    }
}

using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class WpfDbConnectionFactory : IDbConnectionFactory
    {
        public Cirrious.MvvmCross.Plugins.SystemData.IDbConnection Create(string address)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), address);
            return new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteConnection<SQLiteConnection>(path);
        }
    }
}

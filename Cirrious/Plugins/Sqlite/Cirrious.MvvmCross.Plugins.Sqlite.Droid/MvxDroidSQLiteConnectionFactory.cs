// MvxDroidSQLiteConnectionFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using SQLite;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Droid
{
    public class MvxDroidSQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public ISQLiteConnection Create(string address)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return new SQLiteConnection(Path.Combine(path, address));
        }
    }
}
#region Copyright

// <copyright file="MvxWinRTSQLiteConnectionFactory.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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
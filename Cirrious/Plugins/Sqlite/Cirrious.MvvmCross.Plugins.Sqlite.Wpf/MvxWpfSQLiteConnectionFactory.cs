#region Copyright

// <copyright file="MvxWpfSQLiteConnectionFactory.cs" company="Ironshod">
// (c) Copyright Ironshod. http://www.ironshod.co.nz
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Developer - Dave Leaver, Ironshod. http://www.ironshod.co.nz

#endregion

using SQLite;
using System;
using System.IO;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Wpf
{
	public class MvxWpfSQLiteConnectionFactory : ISQLiteConnectionFactory
	{
		public ISQLiteConnection Create(string address)
		{
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), address);
			return new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		}
	}
}

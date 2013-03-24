// <copyright file="MvxConsoleSQLiteConnectionFactory.cs" company="Ironshod">
// (c) Copyright Ironshod. http://www.ironshod.co.nz
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Developer - Dave Leaver, Ironshod. http://www.ironshod.co.nz

using System.IO;
using SQLite;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Console
{
	public class MvxConsoleSQLiteConnectionFactory : ISQLiteConnectionFactory
	{
		public ISQLiteConnection Create(string address)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), address);
			return new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		}
	}
}

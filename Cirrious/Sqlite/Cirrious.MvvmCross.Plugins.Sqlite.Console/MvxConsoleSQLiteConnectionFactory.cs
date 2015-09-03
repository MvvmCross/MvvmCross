// MvxConsoleSQLiteConnectionFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


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

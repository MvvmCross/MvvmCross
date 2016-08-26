using SQLite;

namespace MvvmCross.Plugins.Sqlite
{
    public class SqLiteConfig
    {
        public SqLiteConfig(
            string databaseName,
            bool storeDateTimeAsTicks = true,
			SQLiteOpenFlags? openFlags = null)
        {
            DatabaseName = databaseName;
            StoreDateTimeAsTicks = storeDateTimeAsTicks;
			OpenFlags = openFlags ?? SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;
        }
		
        public string DatabaseName { get; set; }
        public bool StoreDateTimeAsTicks { get; set; }
		public SQLiteOpenFlags OpenFlags { get; set; }
    }
}
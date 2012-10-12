using Cirrious.MvvmCross.Plugins.Sqlite;

namespace SimpleDroidSql
{
    public class ListItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string WhenCreated { get; set; }
    }
}
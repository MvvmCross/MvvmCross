using System.Data;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteConnection<T> : Cirrious.MvvmCross.Plugins.SystemData.IDbConnection
        where T : System.Data.IDbConnection, new()
    {
        private System.Data.IDbConnection conn;

        public SQLiteConnection(string dbName)
        {
            this.conn = new T();
            this.conn.ConnectionString = "Data Source = " + dbName;
        }

        public object InnerConnection
        {
            get
            {
                return conn;
            }
        }

        public void Close()
        {
            this.conn.Close();
        }

        public Cirrious.MvvmCross.Plugins.SystemData.IDbCommand CreateCommand()
        {
            return new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteCommand(this.conn.CreateCommand(), this);
        }

        public void Open()
        {
            this.conn.Open();
        }


        public Cirrious.MvvmCross.Plugins.SystemData.IDbTransaction BeginTransaction()
        {
            return new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteTransaction(this.conn.BeginTransaction(), this);
        }
    }
}

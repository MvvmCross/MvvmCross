using System.Data;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteCommand : Cirrious.MvvmCross.Plugins.SystemData.IDbCommand
    {
        private System.Data.IDbCommand command;
        private Cirrious.MvvmCross.Plugins.SystemData.IDbConnection connection;
        private Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteParameterCollection parameters;

        public SQLiteCommand(System.Data.IDbCommand command, Cirrious.MvvmCross.Plugins.SystemData.IDbConnection connection)
        {
            this.command = command;
            this.connection = connection;

            this.parameters = new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteParameterCollection(command.Parameters, this.command);
        }

        public string CommandText
        {
            get
            {
                return this.command.CommandText;
            }
            set
            {
                this.command.CommandText = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return command.CommandTimeout;
            }
            set
            {
                command.CommandTimeout = value;
            }
        }

        public Cirrious.MvvmCross.Plugins.SystemData.IDbConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
                command.Connection = (System.Data.IDbConnection)connection.InnerConnection;
            }
        }

        public Cirrious.MvvmCross.Plugins.SystemData.IDataParameterCollection Parameters 
        {
            get
            {
                return this.parameters;
            }
        }

        public IDbDataParameter CreateParameter()
        {
            return new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteParameter(command.CreateParameter());
        }

        public int ExecuteNonQuery()
        {
            return command.ExecuteNonQuery();
        }

        public Cirrious.MvvmCross.Plugins.SystemData.IDataReader ExecuteReader()
        {
            return new Cirrious.MvvmCross.Plugins.SystemData.Wpf.SQLiteDataReader(command.ExecuteReader());
        }

        public object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }

        public void Prepare()
        {
            command.Prepare();
        }

        public void Dispose()
        {
            command.Dispose();

        }
    }
}

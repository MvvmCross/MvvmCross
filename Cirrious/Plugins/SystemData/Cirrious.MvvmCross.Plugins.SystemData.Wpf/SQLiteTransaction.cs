using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.SystemData.Wpf
{
    public class SQLiteTransaction : Cirrious.MvvmCross.Plugins.SystemData.IDbTransaction
    {
        Cirrious.MvvmCross.Plugins.SystemData.IDbConnection connection;
        System.Data.IDbTransaction transaction;

        public SQLiteTransaction(System.Data.IDbTransaction innerTransaction, Cirrious.MvvmCross.Plugins.SystemData.IDbConnection connection)
        {
            this.transaction = innerTransaction;
            this.connection = connection;
        }

        public Cirrious.MvvmCross.Plugins.SystemData.IDbConnection Connection
        {
            get { return connection; }
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}

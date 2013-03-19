using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDbCommand : IDisposable
    {

        string CommandText { get; set; }
        int CommandTimeout { get; set; }
        //CommandType CommandType { get; set; }
        IDbConnection Connection { get; set; }
        //IDbTransaction Transaction { get; set; }
        IDataParameterCollection Parameters { get; }
        IDbDataParameter CreateParameter();
        int ExecuteNonQuery();
        IDataReader ExecuteReader();
        object ExecuteScalar();
        void Prepare();
    }
}

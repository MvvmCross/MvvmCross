using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDataReader : IDisposable, IDataRecord
    {

        bool IsClosed { get; }
        int RecordsAffected { get; }
        void Close();
        bool Read();
    }
}

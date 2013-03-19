using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create(string address);
    }
}

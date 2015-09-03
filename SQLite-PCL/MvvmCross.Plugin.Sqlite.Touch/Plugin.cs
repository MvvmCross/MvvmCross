using System;
using System.Collections.Generic;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace MvvmCross.Plugin.Sqlite.Touch
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterType<IMvxSqliteConnectionFactory, TouchSqliteConnectionFactory>();
        }
    }
}

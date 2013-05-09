// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Droid
{
    public class Plugin
        : IMvxPlugin          
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ISQLiteConnectionFactory>(new MvxDroidSQLiteConnectionFactory());
        }
    }
}
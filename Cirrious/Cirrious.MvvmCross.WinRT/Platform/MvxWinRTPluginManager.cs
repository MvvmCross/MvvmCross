#warning Kill this file
/*
using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Plugins;

namespace Cirrious.MvvmCross.WinRT.Platform
{
    public class MvxWinRTPluginManager : MvxBasePluginManager
    {
        private readonly Dictionary<string, Func<IMvxPlugin>> _loaders = new Dictionary<string, Func<IMvxPlugin>>();
 
        public Dictionary<string, Func<IMvxPlugin>> Loaders
        {
            get
            {
                return _loaders;
            }
        }

        #region Overrides of MvxBasePluginManager

        protected override IMvxPlugin LoadPlugin(Type toLoad)
        {
            var pluginName = toLoad.Namespace;
            if (string.IsNullOrEmpty(pluginName))
            {
                throw new MvxException("Invalid plugin type {0}", toLoad);
            }

            Func<IMvxPlugin> loader;
            if (!_loaders.TryGetValue(pluginName, out loader))
            {
                throw new MvxException("plugin not registered for type {0}", pluginName);
            }

            return loader();
        }

        #endregion
    }
}
*/
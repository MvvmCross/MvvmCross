// MvxLoaderBasedPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Plugins;

namespace Cirrious.MvvmCross.Platform
{
    public class MvxLoaderBasedPluginManager : MvxBasePluginManager
    {
        private readonly Dictionary<string, Func<IMvxPlugin>> _loaders = new Dictionary<string, Func<IMvxPlugin>>();

        public Dictionary<string, Func<IMvxPlugin>> Loaders
        {
            get { return _loaders; }
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
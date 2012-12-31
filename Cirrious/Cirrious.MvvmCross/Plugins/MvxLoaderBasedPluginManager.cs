#region Copyright

// <copyright file="MvxLoaderBasedPluginManager.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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
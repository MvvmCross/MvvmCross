#region Copyright

// <copyright file="MvxLoaderPluginRegistry.cs" company="Cirrious">
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

namespace Cirrious.MvvmCross.Platform
{
    public class MvxLoaderPluginRegistry
    {
        private readonly string _pluginPostfix;

        private readonly Dictionary<string, Func<IMvxPlugin>> _loaders;

        public MvxLoaderPluginRegistry(string expectedPostfix, Dictionary<string, Func<IMvxPlugin>> loaders)
        {
            _pluginPostfix = expectedPostfix;
            _loaders = loaders;
        }

        public void AddUnconventionalPlugin(string pluginName, Func<IMvxPlugin> loader)
        {
            _loaders[pluginName] = loader;
        }

        public void AddConventionalPlugin<TPlugin>()
            where TPlugin : IMvxPlugin
        {
            AddConventionalPlugin(typeof (TPlugin));
        }

        public void AddConventionalPlugin(Type plugin)
        {
            var name = plugin.Namespace ?? string.Empty;
            if (!name.EndsWith(_pluginPostfix))
            {
                throw new MvxException(
                    "You must pass in the type of a plugin instance - like 'typeof(Cirrious.MvvmCross.Plugins.Visibility{0}.Plugin)'",
                    _pluginPostfix);
            }

            name = name.Substring(0, name.Length - _pluginPostfix.Length);

            _loaders.Add(
                name,
                () => (IMvxPlugin) Activator.CreateInstance(plugin));
        }
    }
}
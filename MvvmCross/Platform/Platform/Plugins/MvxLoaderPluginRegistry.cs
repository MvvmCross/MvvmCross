// MvxLoaderPluginRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
    public class MvxLoaderPluginRegistry
    {
        private readonly string _pluginPostfix;

        private readonly IDictionary<string, Func<IMvxPlugin>> _loaders;

        public MvxLoaderPluginRegistry(string expectedPostfix, IDictionary<string, Func<IMvxPlugin>> loaders)
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
            AddConventionalPlugin(typeof(TPlugin));
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
                () => (IMvxPlugin)Activator.CreateInstance(plugin));
        }
    }
}
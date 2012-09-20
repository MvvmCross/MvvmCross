#warning Kill this file
/*
#warning Copyright

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Plugins;

namespace Cirrious.MvvmCross.WinRT.Platform
{
#warning This file is duplicated across projects
    public class MvxWinRTPluginLoaderRegistry
    {
        private const string PluginPostfix = ".WinRT";

        private readonly Dictionary<string, Func<IMvxPlugin>> _loaders;

        public MvxWinRTPluginLoaderRegistry(Dictionary<string, Func<IMvxPlugin>> loaders)
        {
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
            if (!name.EndsWith(PluginPostfix))
            {
                throw new MvxException("You must pass in the type of a plugin instance - like 'typeof(Cirrious.MvvmCross.Plugins.Visibility.WinRT.Plugin)'");
            }

            name = name.Substring(0, name.Length - PluginPostfix.Length);

            _loaders.Add(
                name,
                () => (IMvxPlugin)Activator.CreateInstance(plugin));
        }
    }
}
*/
// MvxPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore.Plugins
{
    public abstract class MvxPluginManager
        : IMvxPluginManager
    {
        private readonly Dictionary<Type, IMvxPlugin> _loadedPlugins = new Dictionary<Type, IMvxPlugin>();

        public Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

        public bool IsPluginLoaded<T>() where T : IMvxPluginLoader
        {
            lock (this)
            {
                return _loadedPlugins.ContainsKey(typeof (T));
            }
        }

        public void EnsurePluginLoaded<TType>()
        {
            EnsurePluginLoaded(typeof (TType));
        }

        public virtual void EnsurePluginLoaded(Type type)
        {
            var field = type.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
            if (field == null)
            {
                MvxTrace.Trace("Plugin Instance not found - will not autoload {0}", type.Name);
                return;
            }

            var instance = field.GetValue(null);
            if (instance == null)
            {
                MvxTrace.Trace("Plugin Instance was empty - will not autoload {0}", type.Name);
                return;
            }

            var pluginLoader = instance as IMvxPluginLoader;
            if (pluginLoader == null)
            {
                MvxTrace.Trace("Plugin Instance was not a loader - will not autoload {0}", type.Name);
                return;
            }

            EnsurePluginLoaded(pluginLoader);
        }

        protected virtual void EnsurePluginLoaded(IMvxPluginLoader pluginLoader)
        {
            var configurable = pluginLoader as IMvxConfigurablePluginLoader;
            if (configurable != null)
            {
                MvxTrace.Trace("Configuring Plugin Loader for {0}", pluginLoader.GetType().Name);
                var configuration = ConfigurationFor(pluginLoader.GetType());
                configurable.Configure(configuration);
            }

            MvxTrace.Trace("Ensuring Plugin is loaded for {0}", pluginLoader.GetType().Name);
            pluginLoader.EnsureLoaded();
        }

        public void EnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader
        {
            lock (this)
            {
                if (IsPluginLoaded<T>())
                {
                    return;
                }

                var toLoad = typeof (T);
                _loadedPlugins[toLoad] = ExceptionWrappedLoadPlugin(toLoad);
            }
        }

        private IMvxPlugin ExceptionWrappedLoadPlugin(Type toLoad)
        {
            try
            {
                var plugin = FindPlugin(toLoad);
                var configurablePlugin = plugin as IMvxConfigurablePlugin;
                if (configurablePlugin != null)
                {
                    var configuration = ConfigurationSource(toLoad);
                    configurablePlugin.Configure(configuration);
                }
                plugin.Load();
                return plugin;
            }
            catch (MvxException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap();
            }
        }

        protected IMvxPluginConfiguration ConfigurationFor(Type toLoad)
        {
            if (ConfigurationSource == null)
                return null;

            return ConfigurationSource(toLoad);
        }

        protected abstract IMvxPlugin FindPlugin(Type toLoad);
    }
}
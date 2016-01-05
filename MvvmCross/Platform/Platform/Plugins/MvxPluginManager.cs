// MvxPluginManager.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Plugins
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public abstract class MvxPluginManager
        : IMvxPluginManager
    {
        private readonly Dictionary<Type, IMvxPlugin> _loadedPlugins = new Dictionary<Type, IMvxPlugin>();

        public Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

        public bool IsPluginLoaded<T>() where T : IMvxPluginLoader
        {
            lock (this)
            {
                return this._loadedPlugins.ContainsKey(typeof(T));
            }
        }

        public void EnsurePluginLoaded<TType>()
        {
            this.EnsurePluginLoaded(typeof(TType));
        }

        public virtual void EnsurePluginLoaded(Type type)
        {
            var field = type.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
            if (field == null)
            {
                MvxTrace.Trace("Plugin Instance not found - will not autoload {0}", type.FullName);
                return;
            }

            var instance = field.GetValue(null);
            if (instance == null)
            {
                MvxTrace.Trace("Plugin Instance was empty - will not autoload {0}", type.FullName);
                return;
            }

            var pluginLoader = instance as IMvxPluginLoader;
            if (pluginLoader == null)
            {
                MvxTrace.Trace("Plugin Instance was not a loader - will not autoload {0}", type.FullName);
                return;
            }

            this.EnsurePluginLoaded(pluginLoader);
        }

        protected virtual void EnsurePluginLoaded(IMvxPluginLoader pluginLoader)
        {
            var configurable = pluginLoader as IMvxConfigurablePluginLoader;
            if (configurable != null)
            {
                MvxTrace.Trace("Configuring Plugin Loader for {0}", pluginLoader.GetType().FullName);
                var configuration = this.ConfigurationFor(pluginLoader.GetType());
                configurable.Configure(configuration);
            }

            MvxTrace.Trace("Ensuring Plugin is loaded for {0}", pluginLoader.GetType().FullName);
            pluginLoader.EnsureLoaded();
        }

        public void EnsurePlatformAdaptionLoaded<T>()
            where T : IMvxPluginLoader
        {
            lock (this)
            {
                if (this.IsPluginLoaded<T>())
                {
                    return;
                }

                var toLoad = typeof(T);
                this._loadedPlugins[toLoad] = this.ExceptionWrappedLoadPlugin(toLoad);
            }
        }

        public bool TryEnsurePlatformAdaptionLoaded<T>()
            where T : IMvxPluginLoader
        {
            lock (this)
            {
                if (this.IsPluginLoaded<T>())
                {
                    return true;
                }

                try
                {
                    var toLoad = typeof(T);
                    this._loadedPlugins[toLoad] = this.ExceptionWrappedLoadPlugin(toLoad);
                    return true;
                }
                // pokemon 'catch them all' exception handling allowed here in this Try method
                catch (Exception exception)
                {
                    Mvx.Warning("Failed to load plugin adaption {0} with exception {1}", typeof(T).FullName, exception.ToLongString());
                    return false;
                }
            }
        }

        private IMvxPlugin ExceptionWrappedLoadPlugin(Type toLoad)
        {
            try
            {
                var plugin = this.FindPlugin(toLoad);
                var configurablePlugin = plugin as IMvxConfigurablePlugin;
                if (configurablePlugin != null)
                {
                    var configuration = this.ConfigurationSource(configurablePlugin.GetType());
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

        protected IMvxPluginConfiguration ConfigurationFor(Type toLoad) => this.ConfigurationSource?.Invoke(toLoad);

        protected abstract IMvxPlugin FindPlugin(Type toLoad);
    }
}
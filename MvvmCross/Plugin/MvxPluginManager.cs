﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Plugin
{
    public class MvxPluginManager : IMvxPluginManager
    {
        private readonly HashSet<Type> _loadedPlugins = new HashSet<Type>();

        public Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; }

        public IEnumerable<Type> LoadedPlugins => _loadedPlugins;

        public MvxPluginManager(Func<Type, IMvxPluginConfiguration> configurationSource)
        {
            ConfigurationSource = configurationSource;
        }

        public void EnsurePluginLoaded<TPlugin>(bool forceLoad = false) where TPlugin : IMvxPlugin
        {
            EnsurePluginLoaded(typeof(TPlugin), forceLoad);
        }

        public virtual void EnsurePluginLoaded(Type type, bool forceLoad = false)
        {
            if (forceLoad == false && IsPluginLoaded(type)) return;
            
            var plugin = Activator.CreateInstance(type) as IMvxPlugin;
            if (plugin == null)
                throw new MvxException($"Type {type} is not an IMvxPlugin");

            if (plugin is IMvxConfigurablePlugin configurablePlugin)
            {
                var configuration = ConfigurationFor(type);
                configurablePlugin.Configure(configuration);
            }

            plugin.Load();

            _loadedPlugins.Add(type);
        }

        protected IMvxPluginConfiguration ConfigurationFor(Type toLoad) => ConfigurationSource?.Invoke(toLoad);

        public bool IsPluginLoaded<T>() where T : IMvxPlugin    
            => IsPluginLoaded(typeof(T));

        public bool IsPluginLoaded(Type type)
        {
            lock (this)
            {
                return _loadedPlugins.Contains(type);
            }
        }

        public bool TryEnsurePluginLoaded<TPlugin>(bool forceLoad = false) where TPlugin : IMvxPlugin 
            => TryEnsurePluginLoaded(typeof(TPlugin), forceLoad);

        public bool TryEnsurePluginLoaded(Type type, bool forceLoad = false)
        {
            try
            {
                EnsurePluginLoaded(type, forceLoad);
                return true;
            }
            catch (Exception ex)
            {
                MvxLog.Instance.Warn("Failed to load plugin {0} with exception {1}", type.FullName, ex.ToLongString());
                return false;
            }
        }
    }
}

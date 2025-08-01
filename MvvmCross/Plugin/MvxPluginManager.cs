// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace MvvmCross.Plugin
{
    public class MvxPluginManager : IMvxPluginManager
    {
        private readonly IMvxIoCProvider _provider;
        private readonly object _lockObject = new();
        private readonly HashSet<Type> _loadedPlugins = new();

        public Func<Type, IMvxPluginConfiguration?> ConfigurationSource { get; }

        public IEnumerable<Type> LoadedPlugins => _loadedPlugins;

        public MvxPluginManager(IMvxIoCProvider provider, Func<Type, IMvxPluginConfiguration?> configurationSource)
        {
            _provider = provider;
            ConfigurationSource = configurationSource;
        }

        public void EnsurePluginLoaded<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TPlugin>(bool forceLoad = false) where TPlugin : IMvxPlugin
        {
            EnsurePluginLoaded(typeof(TPlugin), forceLoad);
        }

        public virtual void EnsurePluginLoaded(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type, bool forceLoad = false)
        {
            if (!forceLoad && IsPluginLoaded(type))
                return;

            var plugin = Activator.CreateInstance(type) as IMvxPlugin;
            if (plugin == null)
                throw new MvxException($"Type {type} is not an IMvxPlugin");

            if (plugin is IMvxConfigurablePlugin configurablePlugin)
            {
                var configuration = ConfigurationFor(type);
                if (configuration != null)
                    configurablePlugin.Configure(configuration);
            }

            plugin.Load(_provider);

            lock (_lockObject)
            {
                _loadedPlugins.Add(type);
            }
        }

        protected IMvxPluginConfiguration? ConfigurationFor(Type toLoad) =>
            ConfigurationSource.Invoke(toLoad);

        public bool IsPluginLoaded<T>() where T : IMvxPlugin
            => IsPluginLoaded(typeof(T));

        public bool IsPluginLoaded(Type type)
        {
            lock (_lockObject)
            {
                return _loadedPlugins.Contains(type);
            }
        }

        public bool TryEnsurePluginLoaded<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TPlugin>(bool forceLoad = false) where TPlugin : IMvxPlugin
            => TryEnsurePluginLoaded(typeof(TPlugin), forceLoad);

        public bool TryEnsurePluginLoaded([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type, bool forceLoad = false)
        {
            try
            {
                EnsurePluginLoaded(type, forceLoad);
                return true;
            }
            catch (Exception ex)
            {
                MvxLogHost.Default?.Log(LogLevel.Warning, ex, "Failed to load plugin {FullPluginName}", type.FullName);
                return false;
            }
        }
    }
}

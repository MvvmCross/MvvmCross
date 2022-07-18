// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Plugin
{
#nullable enable
    public interface IMvxPluginManager
    {
        Func<Type, IMvxPluginConfiguration?> ConfigurationSource { get; }

        IEnumerable<Type> LoadedPlugins { get; }

        bool IsPluginLoaded(Type type);

        bool IsPluginLoaded<TPlugin>() where TPlugin : IMvxPlugin;

        void EnsurePluginLoaded(Type type, bool forceLoad = false);

        void EnsurePluginLoaded<TPlugin>(bool forceLoad = false)
            where TPlugin : IMvxPlugin;

        bool TryEnsurePluginLoaded(Type type, bool forceLoad = false);

        bool TryEnsurePluginLoaded<TPlugin>(bool forceLoad = false)
            where TPlugin : IMvxPlugin;
    }
#nullable restore
}

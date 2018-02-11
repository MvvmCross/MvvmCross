// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugin
{
    public interface IMvxPluginManager
    {
        Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; }

        bool IsPluginLoaded(Type type);

        bool IsPluginLoaded<TPlugin>() where TPlugin : IMvxPlugin;

        void EnsurePluginLoaded(Type type);
        
        void EnsurePluginLoaded<TPlugin>() where TPlugin : IMvxPlugin;

        bool TryEnsurePluginLoaded(Type type);
        
        bool TryEnsurePluginLoaded<TPlugin>() where TPlugin : IMvxPlugin;
     }
}

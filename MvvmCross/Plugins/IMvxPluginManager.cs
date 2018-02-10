﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugins
{
    public interface IMvxPluginManager
    {
        Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

        MvxLoaderPluginRegistry Registry { get; }

        bool IsPluginLoaded<T>() where T : IMvxPluginLoader;

        void EnsurePluginLoaded<TType>();

        void EnsurePluginLoaded(Type type);

        void EnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;

        bool TryEnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;
    }
}

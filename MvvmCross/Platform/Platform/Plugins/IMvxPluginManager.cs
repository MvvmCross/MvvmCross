// IMvxPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Plugins
{
    public interface IMvxPluginManager
    {
        Func<Type, IMvxPluginConfiguration> ConfigurationSource { get; set; }

        bool IsPluginLoaded<T>() where T : IMvxPluginLoader;

        void EnsurePluginLoaded<TType>();

        void EnsurePluginLoaded(Type type);

        void EnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;

        bool TryEnsurePlatformAdaptionLoaded<T>() where T : IMvxPluginLoader;
    }
}
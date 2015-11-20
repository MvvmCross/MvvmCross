// MvxLoaderPluginBootstrapAction.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.CrossCore.Plugins
{
    public class MvxLoaderPluginBootstrapAction<TPlugin, TPlatformPlugin>
        : MvxPluginBootstrapAction<TPlugin>
        where TPlugin : IMvxPluginLoader
        where TPlatformPlugin : IMvxPlugin
    {
        protected override void Load(IMvxPluginManager manager)
        {
            PreLoad(manager);
            base.Load(manager);
        }

        protected virtual void PreLoad(IMvxPluginManager manager)
        {
            var loaderManager = manager as IMvxLoaderPluginManager;

            if (loaderManager == null)
            {
                Mvx.Warning(
                    "You should not register a loader plugin bootstrap action when using a non-loader plugin manager");
                return;
            }

            var pluginNamespace = typeof(TPlugin).Namespace;
            if (string.IsNullOrEmpty(pluginNamespace))
            {
                Mvx.Warning("Unable to find namespace for {0} - skipping", typeof(TPlugin).Name);
                return;
            }

            loaderManager.Finders[pluginNamespace] = () => Activator.CreateInstance<TPlatformPlugin>();
        }
    }
}
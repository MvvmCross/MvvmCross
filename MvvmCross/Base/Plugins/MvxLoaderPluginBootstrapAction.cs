// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platform.Plugins
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
            manager.Registry.Register<TPlugin, TPlatformPlugin>();
        }
    }
}
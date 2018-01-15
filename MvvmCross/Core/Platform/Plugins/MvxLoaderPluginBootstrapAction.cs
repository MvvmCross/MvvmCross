// MvxLoaderPluginBootstrapAction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
// MvxPluginBootstrapAction.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Platform;

namespace MvvmCross.Platform.Plugins
{
    public class MvxPluginBootstrapAction<TPlugin>
        : IMvxBootstrapAction
    {
        public virtual void Run()
        {
            Mvx.CallbackWhenRegistered<IMvxPluginManager>(RunAction);
        }

        protected virtual void RunAction()
        {
            var manager = Mvx.Resolve<IMvxPluginManager>();
            Load(manager);
        }

        protected virtual void Load(IMvxPluginManager manager)
        {
            manager.EnsurePluginLoaded<TPlugin>();
        }
    }
}
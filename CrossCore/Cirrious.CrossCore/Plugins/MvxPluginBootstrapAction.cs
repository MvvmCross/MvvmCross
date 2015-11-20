// MvxPluginBootstrapAction.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore.Plugins
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
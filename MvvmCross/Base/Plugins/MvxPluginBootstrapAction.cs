// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform.Platform;

namespace MvvmCross.Base.Plugins
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

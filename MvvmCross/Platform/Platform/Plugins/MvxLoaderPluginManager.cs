// MvxLoaderPluginManager.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Plugins
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Exceptions;

    public class MvxLoaderPluginManager : MvxPluginManager, IMvxLoaderPluginManager
    {
        private readonly Dictionary<string, Func<IMvxPlugin>> _finders = new Dictionary<string, Func<IMvxPlugin>>();

        public IDictionary<string, Func<IMvxPlugin>> Finders => this._finders;

        protected override IMvxPlugin FindPlugin(Type toLoad)
        {
            var pluginName = toLoad.Namespace;
            if (string.IsNullOrEmpty(pluginName))
            {
                throw new MvxException("Invalid plugin type {0}", toLoad);
            }

            Func<IMvxPlugin> finder;
            if (!this._finders.TryGetValue(pluginName, out finder))
            {
                throw new MvxException("plugin not registered for type {0}", pluginName);
            }

            return finder();
        }
    }
}
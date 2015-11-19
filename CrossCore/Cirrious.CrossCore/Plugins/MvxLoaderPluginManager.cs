// MvxLoaderPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using System;
using System.Collections.Generic;

namespace Cirrious.CrossCore.Plugins
{
    public class MvxLoaderPluginManager : MvxPluginManager, IMvxLoaderPluginManager
    {
        private readonly Dictionary<string, Func<IMvxPlugin>> _finders = new Dictionary<string, Func<IMvxPlugin>>();

        public IDictionary<string, Func<IMvxPlugin>> Finders => _finders;

        protected override IMvxPlugin FindPlugin(Type toLoad)
        {
            var pluginName = toLoad.Namespace;
            if (string.IsNullOrEmpty(pluginName))
            {
                throw new MvxException("Invalid plugin type {0}", toLoad);
            }

            Func<IMvxPlugin> finder;
            if (!_finders.TryGetValue(pluginName, out finder))
            {
                throw new MvxException("plugin not registered for type {0}", pluginName);
            }

            return finder();
        }
    }
}
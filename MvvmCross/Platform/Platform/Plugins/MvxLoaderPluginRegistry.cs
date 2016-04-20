// MvxLoaderPluginRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Plugins
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Platform.Exceptions;

    public class MvxLoaderPluginRegistry
    {
        private readonly IDictionary<string, Func<IMvxPlugin>> _loaders = new Dictionary<string, Func<IMvxPlugin>> ();

        public void Register<TPlugin, TPlatformPlugin>()
            where TPlugin : IMvxPluginLoader
            where TPlatformPlugin : IMvxPlugin
        {
            this.Register(typeof(TPlugin), typeof (TPlatformPlugin));
        }

        public void Register(Type plugin, Type platformPlugin)
        {
            this.Register (plugin, () => (IMvxPlugin)Activator.CreateInstance (platformPlugin));
        }

        public void Register<TPlugin> (Func<IMvxPlugin> loader)
            where TPlugin : IMvxPlugin
        {
            this.Register (typeof (TPlugin), loader);
        }

        public void Register(Type plugin, Func<IMvxPlugin> loader)
        {
            var name = plugin.Namespace;
            if (string.IsNullOrEmpty (name)) 
            {
                throw new MvxException("Invalid plugin type {0}", plugin);
            }

            if (loader == null)
            {
                throw new MvxException("Loader function can not be null");
            }

            this._loaders.Add(name, loader);
        }

        public Func<IMvxPlugin> FindLoader (Type plugin)
        {
            Func<IMvxPlugin> loader = null;
            this._loaders.TryGetValue (plugin.Namespace, out loader);
            return loader;
        }
    }
}
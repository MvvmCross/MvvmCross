﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MvvmCross.Exceptions;

namespace MvvmCross.Plugins
{
    public class MvxLoaderPluginRegistry
    {
        private readonly IDictionary<string, Func<IMvxPlugin>> _loaders = new Dictionary<string, Func<IMvxPlugin>>();

        public void Register<TPlugin, TPlatformPlugin>()
            where TPlugin : IMvxPluginLoader
            where TPlatformPlugin : IMvxPlugin
        {
            Register(typeof(TPlugin), typeof(TPlatformPlugin));
        }

        public void Register(Type plugin, Type platformPlugin)
        {
            Register (plugin, () => (IMvxPlugin)Activator.CreateInstance (platformPlugin));
        }

        public void Register<TPlugin>(Func<IMvxPlugin> loader)
            where TPlugin : IMvxPlugin
        {
            Register (typeof(TPlugin), loader);
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

            _loaders.Add(name, loader);
        }

        public Func<IMvxPlugin> FindLoader (Type plugin)
        {
            Func<IMvxPlugin> loader = null;
            _loaders.TryGetValue (plugin.Namespace, out loader);
            return loader;
        }
    }
}

// MvxBasePluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;

namespace Cirrious.MvvmCross.Plugins
{
    public abstract class MvxBasePluginManager
        : IMvxPluginManager
    {
        private readonly Dictionary<Type, IMvxPlugin> _loadedPlugins = new Dictionary<Type, IMvxPlugin>();

        #region Implementation of IMvxPluginManager

        public bool IsPluginLoaded<T>() where T : IMvxPluginLoader
        {
            lock (this)
            {
                return _loadedPlugins.ContainsKey(typeof (T));
            }
        }

        public void EnsureLoaded<T>() where T : IMvxPluginLoader
        {
            lock (this)
            {
                if (IsPluginLoaded<T>())
                {
                    return;
                }

                var toLoad = typeof (T);
                _loadedPlugins[toLoad] = ExceptionWrappedLoadPlugin(toLoad);
            }
        }

        private IMvxPlugin ExceptionWrappedLoadPlugin(Type toLoad)
        {
            try
            {
                var plugin = LoadPlugin(toLoad);
                plugin.Load();
                return plugin;
            }
            catch (MvxException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw exception.MvxWrap();
            }
        }

        protected abstract IMvxPlugin LoadPlugin(Type toLoad);

        #endregion
    }
}
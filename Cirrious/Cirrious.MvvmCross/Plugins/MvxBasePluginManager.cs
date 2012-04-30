#region Copyright
// <copyright file="MvxPluginManager.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Platform.Diagnostics;

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
                return _loadedPlugins.ContainsKey(typeof(T));
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
               
                var toLoad = typeof(T);
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
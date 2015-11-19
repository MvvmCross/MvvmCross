// MvxFilePluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore.Plugins
{
    public class MvxFilePluginManager
        : MvxPluginManager
    {
        private readonly List<string> _platformDllPostfixes;
        private readonly string _assemblyExtension;

        public MvxFilePluginManager(string platformDllPostfix, string assemblyExtension = "")
        {
            _platformDllPostfixes = new List<string>() { platformDllPostfix };
            _assemblyExtension = assemblyExtension;
        }

        public MvxFilePluginManager(List<string> platformDllPostfixes, string assemblyExtension = "")
        {
            _platformDllPostfixes = platformDllPostfixes;
            _assemblyExtension = assemblyExtension;
        }

        protected override IMvxPlugin FindPlugin(Type toLoad)
        {
            var assembly = LoadAssembly(toLoad);

            //var pluginTypes = assembly.ExceptionSafeGetTypes().Select(x => x.FullName);
            //foreach (var type in pluginTypes)
            //{
            //    MvxTrace.Trace("-- Type {0}", type);
            //}

            var pluginType = assembly.ExceptionSafeGetTypes().FirstOrDefault(x => typeof(IMvxPlugin).IsAssignableFrom(x));
            if (pluginType == null)
            {
                throw new MvxException("Could not find plugin type in assembly");
            }

            var pluginObject = (IMvxPlugin)Activator.CreateInstance(pluginType);
            return pluginObject;
        }

        protected virtual Assembly LoadAssembly(Type toLoad)
        {
            foreach (var platformDllPostfix in _platformDllPostfixes)
            {
                var assemblyName = GetPluginAssemblyNameFrom(toLoad, platformDllPostfix);
                MvxTrace.Trace("Loading plugin assembly: {0}", assemblyName);

                try
                {
                    var assembly = Assembly.Load(new AssemblyName(assemblyName));
                    return assembly;
                }
                catch (Exception)
                {
                    //Intentionally ignored
                }
            }

            var error = $"could not load plugin assembly for type {toLoad}";
            throw new MvxException(error);
        }

        protected virtual string GetPluginAssemblyNameFrom(Type toLoad, string platformDllPostfix)
        {
            return $"{toLoad.Namespace}{platformDllPostfix}{_assemblyExtension}";
        }
    }
}
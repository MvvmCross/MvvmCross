﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace MvvmCross.Plugins
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
                MvxLog.Instance.Trace("Loading plugin assembly: {0}", assemblyName);

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

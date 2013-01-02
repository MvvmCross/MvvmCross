// MvxFileBasedPluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.Plugins;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Plugins
{
    public class MvxFileBasedPluginManager
        : MvxBasePluginManager
    {
        private readonly string _platformDllPostfix;
        private readonly string _assemblyExtension;

        public MvxFileBasedPluginManager(string platformDllPostfix, string assemblyExtension = ".dll")
        {
            _platformDllPostfix = platformDllPostfix;
            _assemblyExtension = assemblyExtension;
        }

        #region Overrides of MvxBasePluginManager

        protected override IMvxPlugin LoadPlugin(Type toLoad)
        {
            var assembly = LoadAssembly(toLoad);

            //var pluginTypes = assembly.GetTypes().Select(x => x.FullName);
            //foreach (var type in pluginTypes)
            //{
            //    MvxTrace.Trace("-- Type {0}", type);
            //}

            var pluginType = assembly.GetTypes().FirstOrDefault(x => typeof (IMvxPlugin).IsAssignableFrom(x));
            if (pluginType == null)
            {
                throw new MvxException("Could not find plugin type in assembly");
            }

            var pluginObject = (IMvxPlugin) Activator.CreateInstance(pluginType);
            return pluginObject;
        }

        protected virtual Assembly LoadAssembly(Type toLoad)
        {
            MvxTrace.Trace("Loading plugin for {0}", toLoad.AssemblyQualifiedName);
            var fileName = GetPluginAssemblyNameFrom(toLoad);
            MvxTrace.Trace("- plugin assembly is {0}", fileName);
            var assembly = Assembly.Load(fileName);
            return assembly;
        }

        protected virtual string GetPluginAssemblyNameFrom(Type toLoad)
        {
            return string.Format("{0}.{1}{2}", toLoad.Namespace, _platformDllPostfix, _assemblyExtension);
        }

        #endregion
    }
}
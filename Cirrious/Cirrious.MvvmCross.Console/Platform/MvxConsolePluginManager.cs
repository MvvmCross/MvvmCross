// MvxConsolePluginManager.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;

namespace Cirrious.MvvmCross.Console.Platform
{
    public class MvxConsolePluginManager : MvxFilePluginManager
    {
        public MvxConsolePluginManager()
            : base(".Console", ".dll")
        {
        }

        protected override Assembly LoadAssembly(Type toLoad)
        {
            MvxTrace.Trace("Loading plugin for {0}", toLoad.AssemblyQualifiedName);
            var fileName = GetPluginAssemblyNameFrom(toLoad);
            MvxTrace.Trace("- plugin assembly is {0}", fileName);
            var assembly = Assembly.LoadFrom(fileName);
            return assembly;
        }

        protected override string GetPluginAssemblyNameFrom(Type toLoad)
        {
            var fileName = base.GetPluginAssemblyNameFrom(toLoad);
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(location, fileName);
        }
    }
}
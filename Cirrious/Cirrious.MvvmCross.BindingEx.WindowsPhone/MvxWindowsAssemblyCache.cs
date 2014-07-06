// MvxWindowsAssemblyCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.BindingEx.WindowsShared
{
    public class MvxWindowsAssemblyCache 
        : MvxSingleton<IMvxWindowsAssemblyCache>
          , IMvxWindowsAssemblyCache
    {
        public static void EnsureInitialized()
        {
            if (Instance != null)
                return;

            var instance = new MvxWindowsAssemblyCache();

            if (Instance != instance)
                throw new MvxException("Error initialising MvxWindowsAssemblyCache");
        }

        public MvxWindowsAssemblyCache()
        {
            Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; private set; }
    }
}
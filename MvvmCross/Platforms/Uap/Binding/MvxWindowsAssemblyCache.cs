// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;

namespace MvvmCross.Platforms.Uap.Binding
{
    public class MvxWindowsAssemblyCache : IMvxWindowsAssemblyCache
    {
        private static MvxWindowsAssemblyCache? _instance;

        public static MvxWindowsAssemblyCache? Instance => _instance;

        public static void EnsureInitialized()
        {
            if (_instance != null)
                return;

            _instance = new MvxWindowsAssemblyCache();
        }

        public MvxWindowsAssemblyCache()
        {
            if (_instance == null)
                _instance = this;

            Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Exceptions;

namespace MvvmCross.Platforms.Uap.Binding
{
    public class MvxWindowsAssemblyCache
        : MvxSingleton<IMvxWindowsAssemblyCache>, IMvxWindowsAssemblyCache
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

        public IList<Assembly> Assemblies { get; }
    }
}

// MvxWindowsAssemblyCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.BindingEx.WindowsPhone
{
    using System.Collections.Generic;
    using System.Reflection;

    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Exceptions;

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
            this.Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; private set; }
    }
}
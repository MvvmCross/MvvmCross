// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Base.Exceptions;
using MvvmCross.Base.Logging;

namespace MvvmCross.Base.IoC
{
    public class MvxTypeCache<TType> : IMvxTypeCache<TType>
    {
        public Dictionary<string, Type> LowerCaseFullNameCache { get; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> FullNameCache { get; } = new Dictionary<string, Type>();
        public Dictionary<string, Type> NameCache { get; } = new Dictionary<string, Type>();
        public Dictionary<Assembly, bool> CachedAssemblies { get; } = new Dictionary<Assembly, bool>();

        public void AddAssembly(Assembly assembly)
        {
            try
            {
                if (CachedAssemblies.ContainsKey(assembly))
                return;

                var viewType = typeof(TType);
                var query = assembly.DefinedTypes.Where(ti => ti.IsSubclassOf(viewType)).Select(ti => ti.AsType());

                foreach (var type in query)
                {
                    var fullName = type.FullName;
                    if (!string.IsNullOrEmpty(fullName))
                    {
                        FullNameCache[fullName] = type;
                        LowerCaseFullNameCache[fullName.ToLowerInvariant()] = type;
                    }

                    var name = type.Name;
                    if (!string.IsNullOrEmpty(name))
                        NameCache[name] = type;
                }

                CachedAssemblies[assembly] = true;
            }
            catch (ReflectionTypeLoadException e)
            {
                MvxLog.Instance.Warn("ReflectionTypeLoadException masked during loading of {0} - error {1}",
                    assembly.FullName, e.ToLongString());
            }
        }
    }
}

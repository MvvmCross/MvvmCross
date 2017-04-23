// MvxTypeCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvvmCross.Platform.IoC
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
                Mvx.Warning("ReflectionTypeLoadException masked during loading of {0} - error {1}",
                    assembly.FullName, e.ToLongString());
            }
        }
    }
}
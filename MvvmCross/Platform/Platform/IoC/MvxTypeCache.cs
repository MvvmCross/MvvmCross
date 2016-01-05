// MvxTypeCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class MvxTypeCache<TType> : IMvxTypeCache<TType>
    {
        public Dictionary<string, Type> LowerCaseFullNameCache { get; private set; }
        public Dictionary<string, Type> FullNameCache { get; private set; }
        public Dictionary<string, Type> NameCache { get; private set; }
        public Dictionary<Assembly, bool> CachedAssemblies { get; private set; }

        public MvxTypeCache()
        {
            this.LowerCaseFullNameCache = new Dictionary<string, Type>();
            this.FullNameCache = new Dictionary<string, Type>();
            this.NameCache = new Dictionary<string, Type>();
            this.CachedAssemblies = new Dictionary<Assembly, bool>();
        }

        public void AddAssembly(Assembly assembly)
        {
            if (this.CachedAssemblies.ContainsKey(assembly))
                return;

            var viewType = typeof(TType);
            var query = from type in assembly.ExceptionSafeGetTypes()
                        where viewType.IsAssignableFrom(type)
                        select type;

            foreach (var type in query)
            {
                if (!string.IsNullOrEmpty(type.FullName))
                {
                    this.FullNameCache[type.FullName] = type;
                    this.LowerCaseFullNameCache[type.FullName.ToLowerInvariant()] = type;
                }
                if (!string.IsNullOrEmpty(type.Name))
                {
                    this.NameCache[type.Name] = type;
                }
            }

            this.CachedAssemblies[assembly] = true;
        }
    }
}
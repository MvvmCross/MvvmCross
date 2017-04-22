// MvxTypeCache.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

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
            if (CachedAssemblies.ContainsKey(assembly))
                return;

            var viewType = typeof(TType);
            var query = assembly.DefinedTypes.Where(ti => ti.IsSubclassOf(viewType));

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
                {
                    NameCache[name] = type;
                }
            }

            CachedAssemblies[assembly] = true;
        }
    }
}
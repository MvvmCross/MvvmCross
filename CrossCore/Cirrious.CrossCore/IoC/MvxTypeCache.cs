// MvxTypeCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
    public class MvxTypeCache<TType> : IMvxTypeCache<TType>
    {
        public Dictionary<string, Type> LowerCaseFullNameCache { get; private set; }
        public Dictionary<string, Type> FullNameCache { get; private set; }
        public Dictionary<string, Type> NameCache { get; private set; }
        public Dictionary<Assembly, bool> CachedAssemblies { get; private set; }

        public MvxTypeCache()
        {
            LowerCaseFullNameCache = new Dictionary<string, Type>();
            FullNameCache = new Dictionary<string, Type>();
            NameCache = new Dictionary<string, Type>();
            CachedAssemblies = new Dictionary<Assembly, bool>();
        }

        public void AddAssembly(Assembly assembly)
        {
            if (CachedAssemblies.ContainsKey(assembly))
                return;

            var viewType = typeof(TType);
            var query = from type in assembly.ExceptionSafeGetTypes()
                        where viewType.IsAssignableFrom(type)
                        select type;

            foreach (var type in query)
            {
                if (!string.IsNullOrEmpty(type.FullName))
                {
                    FullNameCache[type.FullName] = type;
                    LowerCaseFullNameCache[type.FullName.ToLowerInvariant()] = type;
                }
                if (!string.IsNullOrEmpty(type.Name))
                {
                    NameCache[type.Name] = type;
                }
            }

            CachedAssemblies[assembly] = true;
        }
    }
}
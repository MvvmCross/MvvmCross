// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Reflection;
using Microsoft.Extensions.Logging;
using MvvmCross.Logging;

namespace MvvmCross.IoC;

public class MvxTypeCache<TType> : IMvxTypeCache
{
    public Dictionary<string, Type> LowerCaseFullNameCache { get; } = new();
    public Dictionary<string, Type> FullNameCache { get; } = new();
    public Dictionary<string, Type> NameCache { get; } = new();
    public Dictionary<Assembly, bool> CachedAssemblies { get; } = new();

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
            MvxLogHost.Default?.Log(LogLevel.Warning, e, "ReflectionTypeLoadException masked during loading of {AssemblyName}",
                assembly.FullName);
        }
    }
}

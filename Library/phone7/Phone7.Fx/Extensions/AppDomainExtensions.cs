using System;
using System.Collections.Generic;
using System.Reflection;

namespace Phone7.Fx.Extensions
{
    public static class AppDomainExtensions
    {
        public static IEnumerable<Assembly> GetManifestIncludedAssemblies(this AppDomain domain)
        {
            var loadedAssemblies = new List<Assembly>();
            var assemblyLister = null as Action<Assembly, List<Assembly>>;
            
            // recurusive lister
            assemblyLister = (Assembly a, List<Assembly>  l) =>
            {
                if (loadedAssemblies.Contains(a)) return;
                loadedAssemblies.Add(a);
                foreach (var m in a.GetModules())
                {
                    if (!loadedAssemblies.Contains(m.Assembly))
                        assemblyLister(m.Assembly, loadedAssemblies);
                }
            };

            // and we load starting with the three assembly references we known off
            assemblyLister(Assembly.GetCallingAssembly(), loadedAssemblies);
            assemblyLister(Assembly.GetExecutingAssembly(), loadedAssemblies);
            assemblyLister(System.Windows.Application.Current.GetType().Assembly, loadedAssemblies);

            return loadedAssemblies;
        }
    }
}
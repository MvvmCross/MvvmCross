// Import.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;

using MvvmCross.Binding.Combiners;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Uwp
{
    public class Import
    {
        static Import()
        {
            MvxDesignTimeChecker.Check();
        }

        private object _from;

        public object From
        {
            get { return _from; }
            set
            {
                if (_from == value)
                    return;

                _from = value;
                if (_from != null)
                {
                    RegisterAssembly(_from.GetType().GetTypeInfo().Assembly);
                }
            }
        }

        private static void RegisterAssembly(Assembly assembly)
        {
            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                MvxWindowsAssemblyCache.EnsureInitialized();
                MvxWindowsAssemblyCache.Instance?.Assemblies.Add(assembly);
            }
            else
            {
                Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(
                    registry =>
                        {
                            registry.AddOrOverwriteFrom(assembly);
                        });
                Mvx.CallbackWhenRegistered<IMvxValueCombinerRegistry>(
                    registry =>
                        {
                            registry.AddOrOverwriteFrom(assembly);
                        });
            }
        }
    }
}
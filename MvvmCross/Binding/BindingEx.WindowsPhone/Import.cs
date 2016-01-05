// Import.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.BindingEx.WindowsPhone
{
    using System.Reflection;

    using MvvmCross.Binding.Combiners;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.IoC;

    public class Import
    {
        static Import()
        {
            MvxDesignTimeChecker.Check();
        }

        private object _from;

        public object From
        {
            get { return this._from; }
            set
            {
                if (this._from == value)
                    return;

                this._from = value;
                if (this._from != null)
                {
#if NETFX_CORE
                    RegisterAssembly(_from.GetType().GetTypeInfo().Assembly);
#else
                    RegisterAssembly(this._from.GetType().Assembly);
#endif
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
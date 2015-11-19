// Import.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Binding.Combiners;
using Cirrious.MvvmCross.BindingEx.WindowsPhone;
using Cirrious.MvvmCross.BindingEx.WindowsShared;

// ReSharper disable CheckNamespace
namespace mvx
// ReSharper restore CheckNamespace
{
    public class Import
    {
        static Import ()
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
#if NETFX_CORE
                    RegisterAssembly(_from.GetType().GetTypeInfo().Assembly);
#else
                    RegisterAssembly(_from.GetType().Assembly);
#endif
                }
            }
        }

        private static void RegisterAssembly(Assembly assembly)
        {
            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                MvxWindowsAssemblyCache.EnsureInitialized();
                MvxWindowsAssemblyCache.Instance.Assemblies.Add(assembly);
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
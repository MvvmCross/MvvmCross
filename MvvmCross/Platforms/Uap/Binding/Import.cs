// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using MvvmCross.Base;
using MvvmCross.Converters;
using MvvmCross.IoC;
using MvvmCross.Binding.Combiners;

namespace MvvmCross.Platforms.Uap.Binding
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
            get 
            { 
                return _from; 
            }
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
                Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(
                    registry =>
                        {
                            registry.AddOrOverwriteFrom(assembly);
                        });
                Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueCombinerRegistry>(
                    registry =>
                        {
                            registry.AddOrOverwriteFrom(assembly);
                        });
            }
        }
    }
}

#region Copyright
// <copyright file="MvxServiceProviderSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Linq;
using System.Reflection;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform
{
    public static class MvxServiceProviderSetup
    {
        public static void Initialize(IMvxIoCProvider iocProvider)
        {
            var serviceProviderType = FindServiceProviderTypeInCurrentAssembly();
            Initialize(serviceProviderType, iocProvider);
        }

        public static void Initialize(Type serviceProviderType, IMvxIoCProvider iocProvider)
        {
            if (MvxServiceProvider.Instance != null)
                throw new MvxException("Service registry already initialized!");

#if NETFX_CORE
            var serviceProviderConstructor = serviceProviderType.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
#else
            var serviceProviderConstructor = serviceProviderType.GetConstructors().FirstOrDefault();
#endif
            if (serviceProviderConstructor == null)
                throw new MvxException("No Service Factory Constructor included in Assembly!");
            var serviceProviderObject = serviceProviderConstructor.Invoke(new object[] {});
            if (serviceProviderObject == null)
                throw new MvxException("Construction of Service Factory failed!");
            var serviceProviderSetup = serviceProviderObject as IMvxServiceProviderSetup;
            if (serviceProviderSetup == null)
                throw new MvxException(
                    "Constructed Service Factory does not support IMvxServiceProviderSetup - type " +
                    serviceProviderObject.GetType().FullName);

            serviceProviderSetup.Initialize(iocProvider);

            if (MvxServiceProvider.Instance == null)
                throw new MvxException("Service registry not initialized!");
        }

        private static Type FindServiceProviderTypeInCurrentAssembly()
        {
#if NETFX_CORE
            var serviceProviderType = typeof (MvxServiceProviderSetup)
                .GetTypeInfo().Assembly
                .DefinedTypes
                .Where(x => x.GetCustomAttributes(typeof(MvxServiceProviderAttribute), false).Any())
                .Select(x => x.AsType())
                .FirstOrDefault();
#else
            var serviceProviderType = typeof (MvxServiceProviderSetup)
                .Assembly
                .GetTypes()
                 .Where(x => x.GetCustomAttributes(typeof (MvxServiceProviderAttribute), false).Any())
                .FirstOrDefault();
#endif

            if (serviceProviderType == null)
                throw new MvxException("No Service Factory Type included in Assembly!");
            return serviceProviderType;
        }
    }
}
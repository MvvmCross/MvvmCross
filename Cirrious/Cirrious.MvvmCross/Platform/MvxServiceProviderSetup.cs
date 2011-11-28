using System;
using System.Linq;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.Interfaces;
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

            var serviceProviderConstructor = serviceProviderType.GetConstructors().FirstOrDefault();
            if (serviceProviderConstructor == null)
                throw new MvxException("No Service Factory Constructor included in Assembly!");
            var serviceProviderObject = serviceProviderConstructor.Invoke(new object[] { });
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
            var serviceProviderType = typeof(MvxServiceProviderSetup)
                .Assembly
                .GetTypes()
                .Where(x => x.GetCustomAttributes(typeof(MvxServiceProviderAttribute), false).Any())
                .FirstOrDefault();

            if (serviceProviderType == null)
                throw new MvxException("No Service Factory Type included in Assembly!");
            return serviceProviderType;
        }
    }
}
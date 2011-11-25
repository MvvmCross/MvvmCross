using System;
using System.Linq;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;

namespace Cirrious.MonoCross.Extensions
{
    public class MXPlatformEx
    {
        public static void Initialise()
        {
            // poor man's IoC here?
            IMXServiceFactory serviceFactory = CreateServiceFactory();

            MXServiceFactorySingleton.Instance.ServiceFactory = serviceFactory;
        }

        private static IMXServiceFactory CreateServiceFactory()
        {
            var serviceFactoryType = typeof (MXPlatformEx)
                .Assembly
                .GetTypes()
                .Where(x => x.GetCustomAttributes(typeof (Platform.MXServiceFactoryAttribute), false).Any())
                .FirstOrDefault();
            if (serviceFactoryType == null)
                throw new InvalidOperationException("No Service Factory Type included in Assembly!");
            var serviceFactoryConstructor = serviceFactoryType.GetConstructors().FirstOrDefault();
            if (serviceFactoryConstructor == null)
                throw new InvalidOperationException("No Service Factory Constructor included in Assembly!");
            var serviceFactoryObject = serviceFactoryConstructor.Invoke(new object[] { });
            if (serviceFactoryObject == null)
                throw new InvalidOperationException("Construction of Service Factory failed!");
            var serviceFactory = serviceFactoryObject as IMXServiceFactory;
            if (serviceFactoryObject == null)
                throw new InvalidOperationException("Constructed Service Factory does not support IMXServiceFactory - type " + serviceFactoryObject.GetType().FullName);
            return serviceFactory;
        }
    }
}

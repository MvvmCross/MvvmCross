#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXPlatformEx.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

#region using

using System;
using System.Linq;
using Cirrious.MonoCross.Extensions.Interfaces;
using Cirrious.MonoCross.Extensions.Platform;

#endregion

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
                .Where(x => x.GetCustomAttributes(typeof (MXServiceFactoryAttribute), false).Any())
                .FirstOrDefault();
            if (serviceFactoryType == null)
                throw new InvalidOperationException("No Service Factory Type included in Assembly!");
            var serviceFactoryConstructor = serviceFactoryType.GetConstructors().FirstOrDefault();
            if (serviceFactoryConstructor == null)
                throw new InvalidOperationException("No Service Factory Constructor included in Assembly!");
            var serviceFactoryObject = serviceFactoryConstructor.Invoke(new object[] {});
            if (serviceFactoryObject == null)
                throw new InvalidOperationException("Construction of Service Factory failed!");
            var serviceFactory = serviceFactoryObject as IMXServiceFactory;
            if (serviceFactoryObject == null)
                throw new InvalidOperationException(
                    "Constructed Service Factory does not support IMXServiceFactory - type " +
                    serviceFactoryObject.GetType().FullName);
            return serviceFactory;
        }
    }
}
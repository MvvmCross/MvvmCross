#region Copyright
// <copyright file="MvxServiceProviderExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxServiceProviderExtensions
    {
        public static TService GetService<TService>(this IMvxServiceConsumer<TService> consumer) where TService : class
        {
            var factory = MvxServiceProvider.Instance;

            if (factory == null)
                return default(TService);

            return factory.GetService<TService>();
        }


        public static void RegisterServiceInstance<TInterface>(this IMvxServiceProducer<TInterface> producer, TInterface service) where TInterface : class
        {
            var registry = MvxServiceProvider.Instance;
            registry.RegisterServiceInstance<TInterface>(service);
        }

        public static void RegisterServiceType<TInterface, TType>(this IMvxServiceProducer<TInterface> producer)
        {
            var registry = MvxServiceProvider.Instance;
            registry.RegisterServiceType<TInterface, TType>();
        }

        /*
        public static void RegisterServiceInstance<TInterface>(this IMvxServiceProducer producer, TInterface service) where TInterface : class
        {
            var registry = MvxServiceProvider.Instance;
            registry.RegisterServiceInstance<TInterface>(service);
        }

        public static void RegisterServiceType<TInterface, TType>(this IMvxServiceProducer producer)
        {
            var registry = MvxServiceProvider.Instance;
            registry.RegisterServiceType<TInterface, TType>();
        }
         */
    }
}
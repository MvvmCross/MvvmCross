#region Copyright

// ----------------------------------------------------------------------
// // <copyright file="MXServiceFactoryExtensions.cs" company="Cirrious">
// //     (c) Copyright Cirrious. http://www.cirrious.com
// //     This source is subject to the Microsoft Public License (Ms-PL)
// //     Please see license.txt on http://opensource.org/licenses/ms-pl.html
// //     All other rights reserved.
// // </copyright>
// // 
// // Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// // ------------------------------------------------------------------------

#endregion

namespace Cirrious.MonoCross.Extensions.Platform
{
    public static class MXServiceFactoryExtensions
    {
        public static TService GetService<TService>(this IMXServiceConsumer consumer) where TService : class
        {
            var factory = MXServiceFactorySingleton.Instance.ServiceFactory;

            if (factory == null)
                return default(TService);

            return factory.CreateService<TService>();
        }
    }
}
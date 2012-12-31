#region Copyright

// <copyright file="IMvxServiceProviderRegistry.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.Interfaces.ServiceProvider
{
    public interface IMvxServiceProviderRegistry : IMvxServiceProvider
    {
        void RegisterServiceType<TInterface, TToConstruct>()
            where TInterface : class
            where TToConstruct : class, TInterface;

        void RegisterServiceInstance<TInterface>(TInterface theObject)
            where TInterface : class;

        void RegisterServiceInstance<TInterface>(Func<TInterface> theConstructor)
            where TInterface : class;
    }
}
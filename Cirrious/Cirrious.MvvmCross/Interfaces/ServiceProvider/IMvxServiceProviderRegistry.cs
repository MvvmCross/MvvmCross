// IMvxServiceProviderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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